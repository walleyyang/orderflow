#region Using declarations
#endregion

//This namespace holds Add ons in this folder and is required. Do not change it. 
using System.Collections.Generic;

namespace NinjaTrader.NinjaScript.AddOns
{
    public sealed class GlobalState
    {
        private static readonly GlobalState instance = new GlobalState();
        public static List<DataBar> DataBars { get; set; }
        public static int MaxBarLookBack { get; set; }
        public static double MedianPointOfControl { get; set; }

        static GlobalState() { }

        private GlobalState()
        {
            DataBars = new List<DataBar>();
        }

        public static GlobalState Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
