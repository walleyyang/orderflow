#region Using declarations
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

//This namespace holds Add ons in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.AddOns.OrderFlow
{
    public sealed class StatsHelper
    {
        public static void SetMedianPointOfControl()
        {
            List<DataBar> dataBars = Helper.GetLastSubsetDataBars();
            List<double> pointOfControls = new List<double>();

            foreach (DataBar dataBar in dataBars)
            {
                pointOfControls.Add(dataBar.pointOfControl);
            }

            GlobalState.OrderFlowStats.MedianPointOfControl = Helper.GetMedian(pointOfControls);
        }

        public static void SetClose(double close)
        {
            GlobalState.OrderFlowStats.CurrentClose = close;
        }

        // Sets the delta changes from the previous bars
        public static void SetCumulativeDeltaChanges()
        {
            List<DataBar> dataBars = Helper.GetLastSubsetDataBars();

            DataBar first = dataBars.First();
            DataBar last = dataBars.Last();

            long cumulativeMaxDelta = 0;
            long cumulativeMinDelta = 0;

            // Add max/min amount to determine cumulative deltas for them
            foreach (DataBar dataBar in dataBars)
            {
                cumulativeMaxDelta += dataBar.maxDelta;
                cumulativeMinDelta += dataBar.minDelta;
            }

            SetCumulativeDeltaChange(dataBars);
            SetCumulativeMaxDeltaChange(first, last, cumulativeMaxDelta);
            SetCumulativeMinDeltaChange(first, last, cumulativeMinDelta);
        }

        private static void SetCumulativeDeltaChange(List<DataBar> dataBars)
        {
            DataBar first = dataBars.First();
            DataBar last = dataBars.Last();

            long cumulativeDeltaChange = GetDeltaChange(first.cumulativeDelta, last.cumulativeDelta);
            double cumulativeDeltaPercent = cumulativeDeltaChange == 0 ? 0 : GetDeltaChangePercent(first.cumulativeDelta, cumulativeDeltaChange);

            GlobalState.OrderFlowStats.FirstCumulativeDelta = first.cumulativeDelta;
            GlobalState.OrderFlowStats.LastCumulativeDelta = last.cumulativeDelta;
            GlobalState.OrderFlowStats.CumulativeDelta.change = cumulativeDeltaChange;
            GlobalState.OrderFlowStats.CumulativeDelta.percent = cumulativeDeltaPercent;
        }

        private static void SetCumulativeMaxDeltaChange(DataBar first, DataBar last, long cumulativeDelta)
        {
            long cumulativeMaxDeltaChange = GetDeltaChange(first.maxDelta, cumulativeDelta);
            double cumulativeMaxDeltaPercent = cumulativeMaxDeltaChange == 0 ? 0 : GetDeltaChangePercent(first.maxDelta, cumulativeMaxDeltaChange);

            GlobalState.OrderFlowStats.FirstCumulativeMaxDelta = first.maxDelta;
            GlobalState.OrderFlowStats.LastCumulativeMaxDelta = last.maxDelta;
            GlobalState.OrderFlowStats.CumulativeMaxDelta.change = cumulativeMaxDeltaChange;
            GlobalState.OrderFlowStats.CumulativeMaxDelta.percent = cumulativeMaxDeltaPercent;
        }

        private static void SetCumulativeMinDeltaChange(DataBar first, DataBar last, long cumulativeDelta)
        {
            long cumulativeMinDeltaChange = GetDeltaChange(first.minDelta, cumulativeDelta);
            double cumulativeMinDeltaPercent = cumulativeMinDeltaChange == 0 ? 0 : GetDeltaChangePercent(first.minDelta, cumulativeMinDeltaChange);

            GlobalState.OrderFlowStats.FirstCumulativeMinDelta = first.minDelta;
            GlobalState.OrderFlowStats.LastCumulativeMinDelta = last.minDelta;
            GlobalState.OrderFlowStats.CumulativeMinDelta.change = cumulativeMinDeltaChange;
            GlobalState.OrderFlowStats.CumulativeMinDelta.percent = cumulativeMinDeltaPercent;
        }

        public static void SetCurrentCumulativeDeltaChanges(long currentCumulativeDelta, long currentCumulativeMaxDelta, long currentCumulativeMinDelta)
        {
            long lastCumulativeDelta = GlobalState.OrderFlowStats.LastCumulativeDelta;
            long cumulativeDeltaChange = GetDeltaChange(lastCumulativeDelta, currentCumulativeDelta);
            double cumulativeDeltaPercent = cumulativeDeltaChange == 0 ? 0 : GetDeltaChangePercent(lastCumulativeDelta, cumulativeDeltaChange);

            // Change between last delta and the current
            GlobalState.OrderFlowStats.CurrentCumulativeDelta.change = cumulativeDeltaChange;
            // Percent change between last delta and the current
            GlobalState.OrderFlowStats.CurrentCumulativeDelta.percent = cumulativeDeltaPercent;
            GlobalState.OrderFlowStats.CurrentCumulativeDelta.currentDelta = currentCumulativeDelta;

            long lastCumulativeMaxDelta = GlobalState.OrderFlowStats.LastCumulativeMaxDelta;
            long cumulativeMaxDeltaChange = GetDeltaChange(lastCumulativeMaxDelta, currentCumulativeMaxDelta);
            double cumulativeMaxDeltaPercent = cumulativeMaxDeltaChange == 0 ? 0 : GetDeltaChangePercent(lastCumulativeMaxDelta, cumulativeMaxDeltaChange);

            GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.change = cumulativeMaxDeltaChange;
            GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.percent = cumulativeMaxDeltaPercent;
            GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.currentDelta = currentCumulativeMaxDelta;

            long lastCumulativeMinDelta = GlobalState.OrderFlowStats.LastCumulativeMinDelta;
            long cumulativeMinDeltaChange = GetDeltaChange(lastCumulativeMinDelta, currentCumulativeMinDelta);
            double cumulativeMinDeltaPercent = cumulativeMinDeltaChange == 0 ? 0 : GetDeltaChangePercent(lastCumulativeMinDelta, cumulativeMinDeltaChange);

            GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.change = cumulativeMinDeltaChange;
            GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.percent = cumulativeMinDeltaPercent;
            GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.currentDelta = currentCumulativeMinDelta;
        }

        // Returns difference between deltas
        private static long GetDeltaChange(long first, long last)
        {
            return first == last ? 0 : last - first;
        }

        private static double GetDeltaChangePercent(long first, double change)
        {
            return change > 0 ? Math.Round(Math.Abs(change / first) * 100, 2) : Math.Round(Math.Abs(change / first) * 100, 2) * -1;
        }
    }
}
