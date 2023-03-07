#region Using declarations
#endregion

//This namespace holds Add ons in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.AddOns.OrderFlow
{
    public sealed class StatsDisplayHelper
    {
        public static void SetStatsDisplay()
        {
            SetMedianPointOfControlDisplay();
            SetCumulativeDeltaDisplay();
            SetCumulativeMaxDeltaDisplay();
            SetCumulativeMinDeltaDisplay();
        }

        private static void SetMedianPointOfControlDisplay()
        {
            GlobalState.OrderFlowStatsDisplay.MedianPointOfControl.display = true;
            GlobalState.OrderFlowStatsDisplay.MedianPointOfControl.labelText = "Median Point of Control";
            GlobalState.OrderFlowStatsDisplay.MedianPointOfControl.text = GlobalState.OrderFlowStats.MedianPointOfControl.ToString();
            GlobalState.OrderFlowStatsDisplay.MedianPointOfControl.direction = Helper.GetDirection(
                GlobalState.OrderFlowStats.MedianPointOfControl,
                GlobalState.OrderFlowStats.CurrentClose);
        }

        private static void SetCumulativeDeltaDisplay()
        {
            string cumulativeDeltaText = GetCumulativeDeltaText(
                GlobalState.OrderFlowStats.CumulativeDelta.change,
                GlobalState.OrderFlowStats.CumulativeDelta.percent);

            string currentCumulativeDeltaText = GetCumulativeDeltaText(
                GlobalState.OrderFlowStats.CurrentCumulativeDelta.change,
                GlobalState.OrderFlowStats.CurrentCumulativeDelta.percent);

            GlobalState.OrderFlowStatsDisplay.CumulativeDelta.display = true;
            GlobalState.OrderFlowStatsDisplay.CumulativeDelta.labelText = "Cumulative Delta";
            GlobalState.OrderFlowStatsDisplay.CumulativeDelta.text = cumulativeDeltaText;
            GlobalState.OrderFlowStatsDisplay.CumulativeDelta.currentText = currentCumulativeDeltaText;
            // Sets direction the percent change and the range
            GlobalState.OrderFlowStatsDisplay.CumulativeDelta.direction = Helper.GetDirection(GlobalState.OrderFlowStats.CumulativeDelta.percent);
            // Sets direction based on last cumulative delta value and the current cumulative delta value
            GlobalState.OrderFlowStatsDisplay.CumulativeDelta.currentDirection = Helper.GetDirection(
                GlobalState.OrderFlowStats.LastCumulativeDelta,
                GlobalState.OrderFlowStats.CurrentCumulativeDelta.currentDelta);
        }

        private static void SetCumulativeMaxDeltaDisplay()
        {
            string cumulativeMaxDeltaText = GetCumulativeDeltaText(
                GlobalState.OrderFlowStats.CumulativeMaxDelta.change,
                GlobalState.OrderFlowStats.CumulativeMaxDelta.percent);

            string currentCumulativeMaxDeltaText = GetCumulativeDeltaText(
                GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.change,
                GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.percent);

            GlobalState.OrderFlowStatsDisplay.CumulativeMaxDelta.display = true;
            GlobalState.OrderFlowStatsDisplay.CumulativeMaxDelta.labelText = "Cumulative Max Delta";
            GlobalState.OrderFlowStatsDisplay.CumulativeMaxDelta.text = cumulativeMaxDeltaText;
            GlobalState.OrderFlowStatsDisplay.CumulativeMaxDelta.currentText = currentCumulativeMaxDeltaText;
            GlobalState.OrderFlowStatsDisplay.CumulativeMaxDelta.direction = Helper.GetDirection(GlobalState.OrderFlowStats.CumulativeMaxDelta.percent);
            GlobalState.OrderFlowStatsDisplay.CumulativeMaxDelta.currentDirection = Helper.GetDirection(
                GlobalState.OrderFlowStats.LastCumulativeMaxDelta,
                GlobalState.OrderFlowStats.CurrentCumulativeMaxDelta.currentDelta);
        }

        private static void SetCumulativeMinDeltaDisplay()
        {
            string cumulativeMinDeltaText = GetCumulativeDeltaText(
                GlobalState.OrderFlowStats.CumulativeMinDelta.change,
                GlobalState.OrderFlowStats.CumulativeMinDelta.percent);

            string currentCumulativeMinDeltaText = GetCumulativeDeltaText(
                GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.change,
                GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.percent);

            GlobalState.OrderFlowStatsDisplay.CumulativeMinDelta.display = true;
            GlobalState.OrderFlowStatsDisplay.CumulativeMinDelta.labelText = "Cumulative Min Delta";
            GlobalState.OrderFlowStatsDisplay.CumulativeMinDelta.text = cumulativeMinDeltaText;
            GlobalState.OrderFlowStatsDisplay.CumulativeMinDelta.currentText = currentCumulativeMinDeltaText;
            GlobalState.OrderFlowStatsDisplay.CumulativeMinDelta.direction = Helper.GetDirection(GlobalState.OrderFlowStats.CumulativeMinDelta.percent);
            GlobalState.OrderFlowStatsDisplay.CumulativeMinDelta.currentDirection = Helper.GetDirection(
                GlobalState.OrderFlowStats.LastCumulativeMinDelta,
                GlobalState.OrderFlowStats.CurrentCumulativeMinDelta.currentDelta);
        }

        private static string GetCumulativeDeltaText(double change, double percent)
        {
            return string.Format("{0}  |  {1}%", change, percent);
        }
    }
}
