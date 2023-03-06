#region Using declarations
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
using NinjaTrader.Gui.Chart;
using NinjaTrader.NinjaScript.AddOns.OrderFlow;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators
{
    public class OrderFlowStats : Indicator
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
                Name = "OrderFlowStats";
                Calculate = Calculate.OnEachTick;
                IsOverlay = true;
                DisplayInDataBox = true;
                DrawOnPricePanel = true;
                DrawHorizontalGridLines = true;
                DrawVerticalGridLines = true;
                PaintPriceMarkers = true;
                ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
                //Disable this property if your indicator requires custom values that cumulate with each new market data event. 
                //See Help Guide for additional information.
                IsSuspendedWhileInactive = true;

                MaxBarLookBack = 5;
                FlatRange = 1.0;

                // Global State
                GlobalState.MaxBarLookBack = MaxBarLookBack;
                GlobalState.FlatRange = FlatRange;
            }
            else if (State == State.Configure)
            {
            }
        }

        protected override void OnBarUpdate()
        {
            // Required bars to trade
            if (CurrentBar < 20)
                return;

            if (IsRealNextBar())
            {
                UpdateDataBars();
                PrintDataBar();
                UpdateGlobalStateForPreviousData();
            }

            UpdateGlobalStateForCurrentData();

        }

        protected override void OnRender(ChartControl chartControl, ChartScale chartScale)
        {
            try
            {
                StatsDisplayData medianPOC = GlobalState.OrderFlowStatsDisplay.MedianPointOfControl;
                StatsDisplayData cumulativeDelta = GlobalState.OrderFlowStatsDisplay.CumulativeDelta;
                StatsDisplayData cumulativeMaxDelta = GlobalState.OrderFlowStatsDisplay.CumulativeMaxDelta;
                StatsDisplayData cumulativeMinDelta = GlobalState.OrderFlowStatsDisplay.CumulativeMinDelta;

                List<StatsDisplayData> stats = new List<StatsDisplayData>
                {
                    medianPOC,
                    cumulativeDelta,
                    cumulativeMaxDelta,
                    cumulativeMinDelta
                };

                RenderStatsBox();

                int startPointY = 30;

                foreach (StatsDisplayData stat in stats)
                {
                    RenderStatsText(startPointY, stat.direction, stat.text);
                    startPointY += 20;
                }
            }
            catch
            {
                RenderStatsText(30, Direction.FLAT, "OrderFlowStats");
                RenderStatsText(50, Direction.FLAT, "Waiting for the strategy to be enabled.");
            }
        }

        private void RenderStatsBox()
        {
            SharpDX.RectangleF rectangleF = new SharpDX.RectangleF(ChartPanel.X + 10, ChartPanel.Y + 25, 390, 90);
            SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, SharpDX.Color.Black);
            RenderTarget.FillRectangle(rectangleF, brush);
            RenderTarget.DrawRectangle(rectangleF, brush);

            brush.Dispose();
        }

        private void RenderStatsText(int startPointY, Direction statDirection, string text)
        {
            SharpDX.Color color = SharpDX.Color.White;

            if (statDirection == Direction.BULLISH)
            {
                color = SharpDX.Color.Green;
            }

            if (statDirection == Direction.BEARISH)
            {
                color = SharpDX.Color.Red;
            }

            SharpDX.Vector2 startPoint = new SharpDX.Vector2(ChartPanel.X + 15, ChartPanel.Y + startPointY);
            SharpDX.DirectWrite.TextFormat textFormat = new SharpDX.DirectWrite.TextFormat(Core.Globals.DirectWriteFactory, "Arial", 14);
            SharpDX.DirectWrite.TextLayout textLayout = new SharpDX.DirectWrite.TextLayout(Core.Globals.DirectWriteFactory, text, textFormat, ChartPanel.W, ChartPanel.H);

            SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, color);

            RenderTarget.DrawTextLayout(startPoint, textLayout, brush);

            textLayout.Dispose();
            textFormat.Dispose();
            brush.Dispose();
        }

        private void PrintDataBar()
        {
            TriggerCustomEvent(o =>
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
            }, null);
        }

        private void PrintStats()
        {
            TriggerCustomEvent(o =>
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
            }, null);
        }

        // IsFirstTickOfBar seems to still use two bars ago from current bar that visually formed.
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

        private void UpdateGlobalStateForPreviousData()
        {
            Helper.SetMedianPointOfControl();
            Helper.SetCumulativeDeltaChanges();
        }

        private void UpdateGlobalStateForCurrentData()
        {
            NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as
                NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

            if (barsType == null)
                return;

            var currentBar = barsType.Volumes[CurrentBar];

            Helper.SetCurrentCumulativeDeltaChanges(currentBar.CumulativeDelta, currentBar.MaxSeenDelta, currentBar.MinSeenDelta);
            Helper.SetStatsDisplay(Close[0]);
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

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private OrderFlowStats[] cacheOrderFlowStats;
		public OrderFlowStats OrderFlowStats(int maxBarLookBack, double flatRange)
		{
			return OrderFlowStats(Input, maxBarLookBack, flatRange);
		}

		public OrderFlowStats OrderFlowStats(ISeries<double> input, int maxBarLookBack, double flatRange)
		{
			if (cacheOrderFlowStats != null)
				for (int idx = 0; idx < cacheOrderFlowStats.Length; idx++)
					if (cacheOrderFlowStats[idx] != null && cacheOrderFlowStats[idx].MaxBarLookBack == maxBarLookBack && cacheOrderFlowStats[idx].FlatRange == flatRange && cacheOrderFlowStats[idx].EqualsInput(input))
						return cacheOrderFlowStats[idx];
			return CacheIndicator<OrderFlowStats>(new OrderFlowStats(){ MaxBarLookBack = maxBarLookBack, FlatRange = flatRange }, input, ref cacheOrderFlowStats);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.OrderFlowStats OrderFlowStats(int maxBarLookBack, double flatRange)
		{
			return indicator.OrderFlowStats(Input, maxBarLookBack, flatRange);
		}

		public Indicators.OrderFlowStats OrderFlowStats(ISeries<double> input , int maxBarLookBack, double flatRange)
		{
			return indicator.OrderFlowStats(input, maxBarLookBack, flatRange);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.OrderFlowStats OrderFlowStats(int maxBarLookBack, double flatRange)
		{
			return indicator.OrderFlowStats(Input, maxBarLookBack, flatRange);
		}

		public Indicators.OrderFlowStats OrderFlowStats(ISeries<double> input , int maxBarLookBack, double flatRange)
		{
			return indicator.OrderFlowStats(input, maxBarLookBack, flatRange);
		}
	}
}

#endregion
