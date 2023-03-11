#region Using declarations
#endregion

//This namespace holds Add ons in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.AddOns.OrderFlow
{
    public class Stats
    {
        public double MedianPointOfControl { get; set; }
        public double CurrentClose { get; set; }
        public long FirstCumulativeDelta { get; set; }
        public long LastCumulativeDelta { get; set; }
        public long FirstCumulativeMaxDelta { get; set; }
        public long LastCumulativeMaxDelta { get; set; }
        public long FirstCumulativeMinDelta { get; set; }
        public long LastCumulativeMinDelta { get; set; }
        public StatsData CumulativeDelta { get; set; }
        public StatsData CumulativeMaxDelta { get; set; }
        public StatsData CumulativeMinDelta { get; set; }
        public StatsData CurrentCumulativeDelta { get; set; }
        public StatsData CurrentCumulativeMaxDelta { get; set; }
        public StatsData CurrentCumulativeMinDelta { get; set; }
        public StatsDataSession LinearRegressionSlope { get; set; }
        public StatsData CurrentLinearRegressionSlope { get; set; }

        public Stats()
        {
            CumulativeDelta = new StatsData();
            CumulativeMaxDelta = new StatsData();
            CumulativeMinDelta = new StatsData();
            CurrentCumulativeDelta = new StatsData();
            CurrentCumulativeMaxDelta = new StatsData();
            CurrentCumulativeMinDelta = new StatsData();
            LinearRegressionSlope = new StatsDataSession();
            CurrentLinearRegressionSlope = new StatsData();
        }
    }

    public class StatsData
    {
        public long change;
        public double percent;
        public dynamic current;
    }

    public class StatsDataSession
    {
        public double sessionHigh;
        public double sessionLow;
    }

    public class StatsDisplay
    {
        public StatsDisplayData MedianPointOfControl { get; set; }
        public StatsDisplayData CumulativeDelta { get; set; }
        public StatsDisplayData CumulativeMaxDelta { get; set; }
        public StatsDisplayData CumulativeMinDelta { get; set; }
        public StatsDisplayData LinearRegressionSlope { get; set; }

        public StatsDisplay()
        {
            MedianPointOfControl = new StatsDisplayData();
            CumulativeDelta = new StatsDisplayData();
            CumulativeMaxDelta = new StatsDisplayData();
            CumulativeMinDelta = new StatsDisplayData();
            LinearRegressionSlope = new StatsDisplayData();
        }
    }

    public class StatsDisplayData
    {
        public bool display;
        public Direction direction;
        public Direction currentDirection;
        public string labelText;
        public string text;
        public string currentText;
    }
}
