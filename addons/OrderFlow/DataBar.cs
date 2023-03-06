#region Using declarations
#endregion

//This namespace holds Add ons in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.AddOns.OrderFlow
{
    public class DataBar
    {
        public int time, barNumber;
        public double pointOfControl;
        public long maxDelta, minDelta, deltaChange, delta, cumulativeDelta, volume;
    }
}
