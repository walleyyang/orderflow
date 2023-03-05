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
        public static void SetMedianPointOfControl()
        {
            List<DataBar> dataBars = GetLastSubsetDataBars();
            List<double> pointOfControls = new List<double>();

            foreach (DataBar dataBar in dataBars)
            {
                pointOfControls.Add(dataBar.pointOfControl);
            }

            GlobalState.OrderFlowStats.MedianPointOfControl = GetMedian(pointOfControls);
        }

        public static void SetCumulativeDeltaChanges()
        {
            List<DataBar> dataBars = GetLastSubsetDataBars();
            DataBar first = dataBars.First();
            DataBar last = dataBars.Last();

            long cumulativeDeltaChange = GetDeltaChange(first.cumulativeDelta, last.cumulativeDelta);
            double cumulativeDeltaPercent = cumulativeDeltaChange == 0 ? 0 : GetDeltaChangePercent(first.cumulativeDelta, cumulativeDeltaChange);

            GlobalState.OrderFlowStats.CumulativeDelta.change = cumulativeDeltaChange;
            GlobalState.OrderFlowStats.CumulativeDelta.percent = cumulativeDeltaPercent;

            long cumulativeMaxDelta = 0;
            long cumulativeMinDelta = 0;

            foreach (DataBar dataBar in dataBars)
            {
                cumulativeMaxDelta += dataBar.maxDelta;
                cumulativeMinDelta += dataBar.minDelta;
            }

            long cumulativeMaxDeltaChange = GetDeltaChange(first.maxDelta, cumulativeMaxDelta);
            double cumulativeMaxDeltaPercent = cumulativeMaxDeltaChange == 0 ? 0 : GetDeltaChangePercent(first.maxDelta, cumulativeMaxDeltaChange);

            GlobalState.OrderFlowStats.CumulativeMaxDelta.change = cumulativeMaxDeltaChange;
            GlobalState.OrderFlowStats.CumulativeMaxDelta.percent = cumulativeMaxDeltaPercent;

            long cumulativeMinDeltaChange = GetDeltaChange(first.minDelta, cumulativeMinDelta);
            double cumulativeMinDeltaPercent = cumulativeMinDeltaChange == 0 ? 0 : GetDeltaChangePercent(first.minDelta, cumulativeMinDeltaChange);

            GlobalState.OrderFlowStats.CumulativeMinDelta.change = cumulativeMinDeltaChange;
            GlobalState.OrderFlowStats.CumulativeMinDelta.percent = cumulativeMinDeltaPercent;
        }

        private static List<DataBar> GetLastSubsetDataBars()
        {
            List<DataBar> dataBarsSubset = Enumerable.Reverse(GlobalState.DataBars).Take(GlobalState.MaxBarLookBack).ToList();

            // Revert subset back for ordering
            return Enumerable.Reverse(dataBarsSubset).ToList();
        }

        // Returns difference between deltas
        private static long GetDeltaChange(long first, long last)
        {
            return first == last ? 0 : last - first;
        }

        private static double GetDeltaChangePercent(long first, double change)
        {
            return change > 0 ? Math.Round(Math.Abs(change / first), 2) : Math.Round(Math.Abs(change / first), 2) * -1;
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
