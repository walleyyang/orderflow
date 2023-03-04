#region Using declarations
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

//This namespace holds Add ons in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.AddOns
{
    public sealed class OrderFlowStrategyHelper
    {
        public static void SetMedianPointOfControl()
        {
            List<DataBar> dataBars = GetLastSubsetDataBars();
            List<double> pointOfControls = new List<double>();

            foreach (DataBar dataBar in dataBars)
            {
                pointOfControls.Add(dataBar.pointOfControl);
            }

            GlobalState.MedianPointOfControl = GetMedian(pointOfControls);
        }

        private static List<DataBar> GetLastSubsetDataBars()
        {
            return Enumerable.Reverse(GlobalState.DataBars).Take(GlobalState.MaxBarLookBack).ToList();
        }

        private static double GetMedian(List<double> numbers)
        {
            double[] unsortedNumbers = new double[GlobalState.MaxBarLookBack];
            int counter = 0;

            foreach (var number in numbers)
            {
                unsortedNumbers[counter] = number;
                counter++;
            }

            Array.Sort(unsortedNumbers);

            return GlobalState.MaxBarLookBack % 2 != 0 ? unsortedNumbers[GlobalState.MaxBarLookBack / 2] : (unsortedNumbers[(GlobalState.MaxBarLookBack - 1) / 2] + unsortedNumbers[GlobalState.MaxBarLookBack / 2]) / 2.0;
        }
    }
}
