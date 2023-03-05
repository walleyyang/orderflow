#region Using declarations
#endregion

//This namespace holds Add ons in this folder and is required. Do not change it. 
using System.Collections.Generic;

namespace NinjaTrader.NinjaScript.AddOns.OrderFlow
{
    public sealed class GlobalState
    {
        private static readonly GlobalState instance = new GlobalState();
        public static List<DataBar> DataBars { get; set; }
        public static int MaxBarLookBack { get; set; }
        public static Stats OrderFlowStats { get; set; }

        static GlobalState() { }

        private GlobalState()
        {
            DataBars = new List<DataBar>();
            OrderFlowStats = new Stats();
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
