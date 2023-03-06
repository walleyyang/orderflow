#region Using declarations
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
using NinjaTrader.Gui.Chart;
using NinjaTrader.NinjaScript.AddOns.OrderFlow;
using System.Collections.Generic;

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
            SharpDX.RectangleF rectangleF = new SharpDX.RectangleF(ChartPanel.X + 10, ChartPanel.Y + 25, 290, 90);
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
