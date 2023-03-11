#region Using declarations
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
using NinjaTrader.Gui.Chart;
using NinjaTrader.NinjaScript.AddOns.OrderFlow;
using System;
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
        private bool _firstSessionBar = false;
        private bool _allowHighLowRegressionSlopeSeen = false;

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
                FlatRange = 10.0;

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
                CheckFirstLastSessionBar();
                UpdateDataBars();
                UpdateGlobalStateForPreviousData();
            }

            UpdateGlobalStateForCurrentData();
        }

        private void CheckFirstLastSessionBar()
        {
            _firstSessionBar = Bars.IsFirstBarOfSession;

            if (_firstSessionBar)
            {
                _allowHighLowRegressionSlopeSeen = true;
            }

            if (Bars.IsLastBarOfSession)
                _allowHighLowRegressionSlopeSeen = false;
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

                // Increase by 20 for each row needed
                int height = 20;
                int heightSpacer = 20;
                int statsBoxHeight = (stats.Count * 20) + heightSpacer + height;
                int extraStatsWidth = 0;

                foreach (StatsDisplayData stat in stats)
                {
                    int maxLength = 0;
                    int length = (stat.labelText + stat.text + stat.currentText).Length;

                    if (length > maxLength)
                    {
                        maxLength = length;
                        extraStatsWidth = maxLength;
                    }
                }

                RenderStatsBox(statsBoxHeight, extraStatsWidth);

                int startPointY = 26;

                foreach (StatsDisplayData stat in stats)
                {
                    if (stat.labelText == "Median Point of Control")
                    {
                        RenderTwoColumns(startPointY, stat.direction, stat.labelText, stat.text);
                    }
                    else
                    {
                        RenderThreeColumns(startPointY, stat.direction, stat.currentDirection, stat.labelText, stat.text, stat.currentText);
                    }

                    startPointY += 20;
                }

                RenderLinearRegressionSlope(startPointY, heightSpacer);
            }
            catch
            {
                Print("Error OnRender");
            }
        }
        private void RenderLinearRegressionSlope(int startPointY, int heightSpacer)
        {
            StatsDisplayData linearRegressionSlope = GlobalState.OrderFlowStatsDisplay.LinearRegressionSlope;
            string labelText = linearRegressionSlope.labelText;
            string text = linearRegressionSlope.text;
            string currentText = linearRegressionSlope.currentText;

            RenderThreeColumns(startPointY + heightSpacer, Direction.FLAT, linearRegressionSlope.currentDirection, labelText, text, currentText);
        }

        #region SharpDX Rendering

        private void RenderStatsBox(int statsBoxHeight, int extraStatsWidth)
        {
            int defaultStatsWidth = 450;
            int defaultMinRowTextLength = 60;
            int statsWidth = extraStatsWidth > defaultMinRowTextLength ? defaultStatsWidth + extraStatsWidth : defaultStatsWidth;

            SharpDX.RectangleF rectangleF = new SharpDX.RectangleF(ChartPanel.X + 10, ChartPanel.Y + 25, statsWidth, statsBoxHeight);
            SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, SharpDX.Color.Black);
            RenderTarget.FillRectangle(rectangleF, brush);
            RenderTarget.DrawRectangle(rectangleF, brush);

            brush.Dispose();
        }

        private void RenderTwoColumns(int startPointY, Direction direction, string labelText, string text)
        {
            SharpDX.Vector2 labelTextStartPoint = GetStartPoint(15, startPointY);
            SharpDX.Vector2 textStartPoint = GetStartPoint(180, startPointY);

            SharpDX.DirectWrite.TextFormat textFormat = GetTextFormat();

            SharpDX.DirectWrite.TextLayout labelTextLayout = GetTextLayout(textFormat, labelText);
            SharpDX.DirectWrite.TextLayout textLayout = GetTextLayout(textFormat, text);

            SharpDX.Direct2D1.SolidColorBrush labelTextBrush = GetBrush(Direction.FLAT);
            SharpDX.Direct2D1.SolidColorBrush textBrush = GetBrush(direction);

            RenderTarget.DrawTextLayout(labelTextStartPoint, labelTextLayout, labelTextBrush);
            RenderTarget.DrawTextLayout(textStartPoint, textLayout, textBrush);

            textFormat.Dispose();
            labelTextLayout.Dispose();
            textLayout.Dispose();
            labelTextBrush.Dispose();
            textBrush.Dispose();
        }

        private void RenderThreeColumns(int startPointY, Direction direction, Direction currentDirection, string labelText, string text, string currentText)
        {
            SharpDX.Vector2 labelTextStartPoint = GetStartPoint(15, startPointY);
            SharpDX.Vector2 textStartPoint = GetStartPoint(180, startPointY);
            SharpDX.Vector2 currentTextStartPoint = GetStartPoint(320, startPointY);

            SharpDX.DirectWrite.TextFormat textFormat = GetTextFormat();

            SharpDX.DirectWrite.TextLayout labelTextLayout = GetTextLayout(textFormat, labelText);
            SharpDX.DirectWrite.TextLayout textLayout = GetTextLayout(textFormat, text);
            SharpDX.DirectWrite.TextLayout currentTextLayout = GetTextLayout(textFormat, currentText);

            SharpDX.Direct2D1.SolidColorBrush labelTextBrush = GetBrush(Direction.FLAT);
            SharpDX.Direct2D1.SolidColorBrush textBush = GetBrush(direction);
            SharpDX.Direct2D1.SolidColorBrush currentTextBrush = GetBrush(currentDirection);

            RenderTarget.DrawTextLayout(labelTextStartPoint, labelTextLayout, labelTextBrush);
            RenderTarget.DrawTextLayout(textStartPoint, textLayout, textBush);
            RenderTarget.DrawTextLayout(currentTextStartPoint, currentTextLayout, currentTextBrush);

            textFormat.Dispose();
            labelTextLayout.Dispose();
            textLayout.Dispose();
            currentTextLayout.Dispose();
            labelTextBrush.Dispose();
            textBush.Dispose();
            currentTextBrush.Dispose();
        }

        private SharpDX.Vector2 GetStartPoint(int xOffset, int yOffset)
        {
            return new SharpDX.Vector2(ChartPanel.X + xOffset, ChartPanel.Y + yOffset);
        }

        private SharpDX.DirectWrite.TextFormat GetTextFormat()
        {
            return new SharpDX.DirectWrite.TextFormat(Core.Globals.DirectWriteFactory, "Arial", 14);
        }

        private SharpDX.DirectWrite.TextLayout GetTextLayout(SharpDX.DirectWrite.TextFormat textFormat, string text)
        {
            return new SharpDX.DirectWrite.TextLayout(Core.Globals.DirectWriteFactory, text, textFormat, ChartPanel.W, ChartPanel.H);
        }

        private SharpDX.Direct2D1.SolidColorBrush GetBrush(Direction direction)
        {
            return new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, GetColor(direction));
        }

        private SharpDX.Color GetColor(Direction direction)
        {
            SharpDX.Color color = SharpDX.Color.White;

            if (direction == Direction.BULLISH)
            {
                color = SharpDX.Color.Green;
            }

            if (direction == Direction.BEARISH)
            {
                color = SharpDX.Color.Red;
            }

            return color;
        }

        #endregion

        #region Print

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
            Print(string.Format("Linear Regression Slope: {0}", dataBar.linearRegressionSlope));
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
            Print(string.Format("Session Linear Regression Slope High: {0}", GlobalState.OrderFlowStats.LinearRegressionSlope.sessionHigh));
            Print(string.Format("Session Linear Regression Slope Low: {0}", GlobalState.OrderFlowStats.LinearRegressionSlope.sessionLow));
            Print("\n");
        }

        #endregion

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
            StatsHelper.SetMedianPointOfControl();
            StatsHelper.SetCumulativeDeltaChanges();

            if (_allowHighLowRegressionSlopeSeen)
            {
                StatsHelper.SetSessionHighLowLinearRegressionSlope(_firstSessionBar);
            }
        }

        private void UpdateGlobalStateForCurrentData()
        {
            NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as
                NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

            if (barsType == null)
                return;

            var currentBar = barsType.Volumes[CurrentBar];

            StatsHelper.SetClose(Close[0]);
            StatsHelper.SetCurrentCumulativeDeltaChanges(currentBar.CumulativeDelta, currentBar.MaxSeenDelta, currentBar.MinSeenDelta);
            StatsHelper.SetCurrentLinearRegressionSlope(Math.Round(LinRegSlope(14)[0], 5));

            StatsDisplayHelper.SetStatsDisplay();
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
            dataBar.linearRegressionSlope = Math.Round(LinRegSlope(14)[offsetBar], 5);

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
