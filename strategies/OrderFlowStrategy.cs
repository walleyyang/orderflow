#region Using declarations
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript.AddOns.OrderFlow;
using System.ComponentModel.DataAnnotations;
using System.Linq;
#endregion

//This namespace holds Strategies in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Strategies
{
    public class OrderFlowStrategy : Strategy
    {
        #region Variables

        private const string GROUP_NAME = "OrderFlow";

        private int _currentBarNumber = 0;

        #endregion

        #region Properties

        [NinjaScriptProperty]
        [Display(Name = "MaxBarLookBack", Description = "The maximum bar to look back starting with previous bar.", Order = 0, GroupName = GROUP_NAME)]
        public int MaxBarLookBack { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "FlatRange", Description = "The range to be considered flat. The range is the entered value from negative to postive.", Order = 1, GroupName = GROUP_NAME)]
        public double FlatRange { get; set; }

        #endregion

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"";
                Name = "OrderFlowStrategy";
                Calculate = Calculate.OnEachTick;
                EntriesPerDirection = 1;
                EntryHandling = EntryHandling.AllEntries;
                IsExitOnSessionCloseStrategy = true;
                ExitOnSessionCloseSeconds = 30;
                IsFillLimitOnTouch = false;
                MaximumBarsLookBack = MaximumBarsLookBack.TwoHundredFiftySix;
                OrderFillResolution = OrderFillResolution.Standard;
                Slippage = 0;
                StartBehavior = StartBehavior.WaitUntilFlat;
                TimeInForce = TimeInForce.Gtc;
                TraceOrders = false;
                RealtimeErrorHandling = RealtimeErrorHandling.StopCancelClose;
                StopTargetHandling = StopTargetHandling.PerEntryExecution;
                BarsRequiredToTrade = 20;
                // Disable this property for performance gains in Strategy Analyzer optimizations
                // See the Help Guide for additional information
                IsInstantiatedOnEachOptimizationIteration = true;

                MaxBarLookBack = 5;
                FlatRange = 1.0;

                // Global State
                GlobalState.MaxBarLookBack = MaxBarLookBack;
                GlobalState.FlatRange = FlatRange;
            }
        }

        protected override void OnBarUpdate()
        {
            if (CurrentBar < BarsRequiredToTrade)
                return;

            if (IsRealNextBar())
            {
                UpdateDataBars();
                UpdateGlobalState();
            }
        }

        private void PrintDataBar()
        {
            DataBar dataBar = GlobalState.DataBars.Last();

            Print(string.Format("{0} | {1}", ToDay(Time[0]), ToTime(Time[0])));
            Print(string.Format("Time: {0}", dataBar.time));
            Print(string.Format("Bar Number: {0}", dataBar.barNumber));
            Print(string.Format("Volume: {0}", dataBar.volume));
            Print(string.Format("Delta: {0}", dataBar.delta));
            Print(string.Format("Max Delta: {0}", dataBar.maxDelta));
            Print(string.Format("Min Delta: {0}", dataBar.minDelta));
            Print(string.Format("Delta Change: {0}", dataBar.deltaChange));
            Print(string.Format("Cumulative Delta: {0}", dataBar.cumulativeDelta));
            Print(string.Format("Point of Control: {0}", dataBar.pointOfControl));
            Print("\n");
        }

        private void PrintStats()
        {
            Print(string.Format("{0} | {1}", ToDay(Time[0]), ToTime(Time[0])));
            Print(string.Format("Median Point of Control: {0}", GlobalState.OrderFlowStats.MedianPointOfControl));
            Print(string.Format("Cumulative Delta Change: {0}", GlobalState.OrderFlowStats.CumulativeDelta.change));
            Print(string.Format("Cumulative Delta Change Percent: {0}", GlobalState.OrderFlowStats.CumulativeDelta.percent));
            Print(string.Format("Cumulative Max Delta Change: {0}", GlobalState.OrderFlowStats.CumulativeMaxDelta.change));
            Print(string.Format("Cumulative Max Delta Change Percent: {0}", GlobalState.OrderFlowStats.CumulativeMaxDelta.percent));
            Print(string.Format("Cumulative Min Delta Change: {0}", GlobalState.OrderFlowStats.CumulativeMinDelta.change));
            Print(string.Format("Cumulative Min Delta Change Percent: {0}", GlobalState.OrderFlowStats.CumulativeMinDelta.percent));
            Print("\n");
        }

        // IsFirstTickOfBar seems to still somtimes use two bars ago from current bar that visually formed.
        // This will check the bar number instead of using IsFirstTickOfBar
        private bool IsRealNextBar()
        {
            if (GlobalState.DataBars.Count == 0)
            {
                _currentBarNumber = CurrentBar;

                return true;
            }

            // Make sure the current bar is a new bar and not the previous bar
            if (CurrentBar > _currentBarNumber)
            {
                _currentBarNumber = CurrentBar;

                return true;
            }

            return false;
        }

        private void UpdateGlobalState()
        {
            Helper.SetMedianPointOfControl();
            Helper.SetCumulativeDeltaChanges();
            Helper.SetStatsDisplay(Open[0]);
        }

        private void UpdateDataBars()
        {
            NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as
                NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

            if (barsType == null)
                return;

            DataBar dataBar = new DataBar();

            // This gets called in first tick of volumetric bar and needs to be offset to ignore current
            int offsetBar = 1;
            var previousVolumetricBar = barsType.Volumes[CurrentBar - offsetBar];
            long delta = previousVolumetricBar.BarDelta;
            long maxDelta = previousVolumetricBar.MaxSeenDelta;
            long minDelta = previousVolumetricBar.MinSeenDelta;
            double dataBarPointOfControl;
            previousVolumetricBar.GetMaximumVolume(null, out dataBarPointOfControl);

            dataBar.time = ToTime(Time[offsetBar]);
            dataBar.barNumber = CurrentBar - offsetBar;
            dataBar.volume = previousVolumetricBar.TotalVolume;
            dataBar.delta = delta;
            dataBar.maxDelta = maxDelta;
            dataBar.minDelta = minDelta;
            dataBar.deltaChange = delta - barsType.Volumes[CurrentBar - (offsetBar + 1)].BarDelta;
            dataBar.cumulativeDelta = previousVolumetricBar.CumulativeDelta;
            dataBar.pointOfControl = dataBarPointOfControl;

            GlobalState.DataBars.Add(dataBar);
        }
    }
}
