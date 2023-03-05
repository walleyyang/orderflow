#region Using declarations
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
using NinjaTrader.NinjaScript.AddOns.OrderFlow;
using NinjaTrader.NinjaScript.DrawingTools;
using System.Windows.Media;

namespace NinjaTrader.NinjaScript.Indicators
{
    public class OrderFlowStats : Indicator
    {
        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"";
                Name = "OrderFlowStats";
                Calculate = Calculate.OnBarClose;
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
            }
            else if (State == State.Configure)
            {
            }
        }

        protected override void OnBarUpdate()
        {
            string medianPOC = string.Format("Median Point of Control:  {0}", GlobalState.OrderFlowStats.MedianPointOfControl);
            string cumulativeDelta = string.Format("Cumulative Delta:  {0}  |  {1}%", GlobalState.OrderFlowStats.CumulativeDelta.change, GlobalState.OrderFlowStats.CumulativeDelta.percent);
            string cumulativeMaxDelta = string.Format("Cumulative Max Delta:  {0}  |  {1}%", GlobalState.OrderFlowStats.CumulativeMaxDelta.change, GlobalState.OrderFlowStats.CumulativeMaxDelta.percent);
            string cumulativeMinDelta = string.Format("Cumulative Min Delta:  {0}  |  {1}%", GlobalState.OrderFlowStats.CumulativeMinDelta.change, GlobalState.OrderFlowStats.CumulativeMinDelta.percent);

            string text = string.Format("{0}\n{1}\n{2}\n{3}", medianPOC, cumulativeDelta, cumulativeMaxDelta, cumulativeMinDelta);

            Draw.TextFixed(
                this,
                "OrderFlowStats",
                text,
                TextPosition.TopLeft,
                Brushes.White,
                new NinjaTrader.Gui.Tools.SimpleFont("Arial ", 12) { Size = 14, Bold = true },
                Brushes.Transparent,
                Brushes.Black,
                100
            );
        }
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private OrderFlowStats[] cacheOrderFlowStats;
		public OrderFlowStats OrderFlowStats()
		{
			return OrderFlowStats(Input);
		}

		public OrderFlowStats OrderFlowStats(ISeries<double> input)
		{
			if (cacheOrderFlowStats != null)
				for (int idx = 0; idx < cacheOrderFlowStats.Length; idx++)
					if (cacheOrderFlowStats[idx] != null &&  cacheOrderFlowStats[idx].EqualsInput(input))
						return cacheOrderFlowStats[idx];
			return CacheIndicator<OrderFlowStats>(new OrderFlowStats(), input, ref cacheOrderFlowStats);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.OrderFlowStats OrderFlowStats()
		{
			return indicator.OrderFlowStats(Input);
		}

		public Indicators.OrderFlowStats OrderFlowStats(ISeries<double> input )
		{
			return indicator.OrderFlowStats(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.OrderFlowStats OrderFlowStats()
		{
			return indicator.OrderFlowStats(Input);
		}

		public Indicators.OrderFlowStats OrderFlowStats(ISeries<double> input )
		{
			return indicator.OrderFlowStats(input);
		}
	}
}

#endregion
