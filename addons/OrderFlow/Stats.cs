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
}
