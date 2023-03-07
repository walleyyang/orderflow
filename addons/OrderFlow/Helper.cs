#region Using declarations
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

//This namespace holds Add ons in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.AddOns.OrderFlow
{
    public sealed class Helper
    {
        public static Direction GetDirection(double value, double comparisonValue = 0)
        {
            if (comparisonValue != 0)
            {
                // Compare value with another value
                if (value < comparisonValue)
                    return Direction.BULLISH;

                if (value > comparisonValue)
                    return Direction.BEARISH;
            }
            else
            {
                // Compare value with flat range
                double maxFlatRange = Math.Abs(GlobalState.FlatRange);
                double minFlatRange = Math.Abs(GlobalState.FlatRange) * -1;

                if (value > maxFlatRange)
                    return Direction.BULLISH;

                if (value < minFlatRange)
                    return Direction.BEARISH;
            }

            return Direction.FLAT;
        }

        public static List<DataBar> GetLastSubsetDataBars()
        {
            List<DataBar> dataBarsSubset = Enumerable.Reverse(GlobalState.DataBars).Take(GlobalState.MaxBarLookBack).ToList();

            // Revert subset back for ordering
            return Enumerable.Reverse(dataBarsSubset).ToList();
        }

        public static double GetMedian(List<double> numbers)
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
