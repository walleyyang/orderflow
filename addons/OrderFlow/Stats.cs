#region Using declarations
#endregion

//This namespace holds Add ons in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.AddOns.OrderFlow
{
    public class Stats
    {
        public double MedianPointOfControl { get; set; }
        public CumulativeDelta CumulativeDelta { get; set; }
        public CumulativeDelta CumulativeMaxDelta { get; set; }
        public CumulativeDelta CumulativeMinDelta { get; set; }

        public Stats()
        {
            CumulativeDelta = new CumulativeDelta();
            CumulativeMaxDelta = new CumulativeDelta();
            CumulativeMinDelta = new CumulativeDelta();
        }
    }

    public class CumulativeDelta
    {
        public long change;
        public double percent;
    }

    public class StatsDisplay
    {
        public StatsDisplayData MedianPointOfControl { get; set; }
        public StatsDisplayData CumulativeDelta { get; set; }
        public StatsDisplayData CumulativeMaxDelta { get; set; }
        public StatsDisplayData CumulativeMinDelta { get; set; }

        public StatsDisplay()
        {
            MedianPointOfControl = new StatsDisplayData();
            CumulativeDelta = new StatsDisplayData();
            CumulativeMaxDelta = new StatsDisplayData();
            CumulativeMinDelta = new StatsDisplayData();
        }
    }

    public class StatsDisplayData
    {
        public bool display;
        public Direction direction;
        public string text;
    }
}
