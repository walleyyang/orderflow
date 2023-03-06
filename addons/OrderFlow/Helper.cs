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

            GlobalState.OrderFlowStats.FirstCumulativeDelta = first.cumulativeDelta;
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

            GlobalState.OrderFlowStats.FirstCumulativeMaxDelta = first.maxDelta;
            GlobalState.OrderFlowStats.CumulativeMaxDelta.change = cumulativeMaxDeltaChange;
            GlobalState.OrderFlowStats.CumulativeMaxDelta.percent = cumulativeMaxDeltaPercent;

            long cumulativeMinDeltaChange = GetDeltaChange(first.minDelta, cumulativeMinDelta);
            double cumulativeMinDeltaPercent = cumulativeMinDeltaChange == 0 ? 0 : GetDeltaChangePercent(first.minDelta, cumulativeMinDeltaChange);

            GlobalState.OrderFlowStats.FirstCumulativeMinDelta = first.minDelta;
            GlobalState.OrderFlowStats.CumulativeMinDelta.change = cumulativeMinDeltaChange;
            GlobalState.OrderFlowStats.CumulativeMinDelta.percent = cumulativeMinDeltaPercent;
        }

        public static void SetCurrentCumulativeDeltaChanges(long currentCumulativeDelta, long currentCumulativeMaxDelta, long currentCumulativeMinDelta)
        {
            long firstCumulativeDelta = GlobalState.OrderFlowStats.FirstCumulativeDelta;
            long cumulativeDeltaChange = GetDeltaChange(firstCumulativeDelta, currentCumulativeDelta);
            double cumulativeDeltaPercent = cumulativeDeltaChange == 0 ? 0 : GetDeltaChangePercent(firstCumulativeDelta, cumulativeDeltaChange);

            GlobalState.OrderFlowStats.CurrentCumulativeDelta.change = cumulativeDeltaChange;
            GlobalState.OrderFlowStats.CurrentCumulativeDelta.percent = cumulativeDeltaPercent;

            long firstCumulativeMaxDelta = GlobalState.OrderFlowStats.FirstCumulativeMaxDelta;
            long cumulativeMaxDeltaChange = GetDeltaChange(firstCumulativeMaxDelta, currentCumulativeMaxDelta);
            double cumulativeMaxDeltaPercent = cumulativeMaxDeltaChange == 0 ? 0 : GetDeltaChangePercent(firstCumulativeMaxDelta, cumulativeMaxDeltaChange);

            GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.change = cumulativeMaxDeltaChange;
            GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.percent = cumulativeMaxDeltaPercent;

            long firstCumulativeMinDelta = GlobalState.OrderFlowStats.FirstCumulativeMinDelta;
            long cumulativeMinDeltaChange = GetDeltaChange(firstCumulativeMinDelta, currentCumulativeMinDelta);
            double cumulativeMinDeltaPercent = cumulativeMinDeltaChange == 0 ? 0 : GetDeltaChangePercent(firstCumulativeMinDelta, cumulativeMinDeltaChange);

            GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.change = cumulativeMinDeltaChange;
            GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.percent = cumulativeMinDeltaPercent;
        }

        public static void SetStatsDisplay(double close)
        {
            string medianPOC = string.Format("Median Point of Control:  {0}", GlobalState.OrderFlowStats.MedianPointOfControl);
            string cumulativeDelta = string.Format("Cumulative Delta:  {0}  |  {1}%  |  {2}  |  {3}%",
                GlobalState.OrderFlowStats.CumulativeDelta.change,
                GlobalState.OrderFlowStats.CumulativeDelta.percent,
                GlobalState.OrderFlowStats.CurrentCumulativeDelta.change,
                GlobalState.OrderFlowStats.CurrentCumulativeDelta.percent);
            string cumulativeMaxDelta = string.Format("Cumulative Max Delta  {0}  |  {1}%  |  {2}  |  {3}%",
                GlobalState.OrderFlowStats.CumulativeMaxDelta.change,
                GlobalState.OrderFlowStats.CumulativeMaxDelta.percent,
                GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.change,
                GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.percent);
            string cumulativeMinDelta = string.Format("Cumulative Min Delta:  {0}  |  {1}%  |  {2}  |  {3}%",
                GlobalState.OrderFlowStats.CumulativeMinDelta.change,
                GlobalState.OrderFlowStats.CumulativeMinDelta.percent,
                GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.change,
                GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.percent);

            GlobalState.OrderFlowStatsDisplay.MedianPointOfControl.display = true;
            GlobalState.OrderFlowStatsDisplay.MedianPointOfControl.text = medianPOC;
            GlobalState.OrderFlowStatsDisplay.MedianPointOfControl.direction = GetDirection(GlobalState.OrderFlowStats.MedianPointOfControl, close);

            GlobalState.OrderFlowStatsDisplay.CumulativeDelta.display = true;
            GlobalState.OrderFlowStatsDisplay.CumulativeDelta.text = cumulativeDelta;
            GlobalState.OrderFlowStatsDisplay.CumulativeDelta.direction = GetDirection(GlobalState.OrderFlowStats.CurrentCumulativeDelta.percent);

            GlobalState.OrderFlowStatsDisplay.CumulativeMaxDelta.display = true;
            GlobalState.OrderFlowStatsDisplay.CumulativeMaxDelta.text = cumulativeMaxDelta;
            GlobalState.OrderFlowStatsDisplay.CumulativeMaxDelta.direction = GetDirection(GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.percent);

            GlobalState.OrderFlowStatsDisplay.CumulativeMinDelta.display = true;
            GlobalState.OrderFlowStatsDisplay.CumulativeMinDelta.text = cumulativeMinDelta;
            GlobalState.OrderFlowStatsDisplay.CumulativeMinDelta.direction = GetDirection(GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.percent);
        }

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
