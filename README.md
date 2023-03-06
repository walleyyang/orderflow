# Important
***Requires lifetime NinjaTrader 8 license that contains the volumetric data.*** The files in the addons, indicators folders can simply be copied over to the respective NinjaTrader folders. Re-compile by pressing F5 in the editor and indicator should display in their list.

# OrderFlowStats
<p align="center">
  <img src="https://raw.githubusercontent.com/walleyyang/orderflow/main/images/orderflowstats.png">
</p>

Displays stats about the current orderflow. The following are the current displayed stats:
- ***Median Point of Control:*** Displays the median point of control for the previous bar to the MaxBarLookBack. Red text represents the open price of the current bar is below the median point of control. Green text represents the open price of the current bar is above the median point of control. White represents the current bar open is the same price as the median point of control.

***Cumulative Deltas:*** Displays the percent changed from the MaxBarLookBack to the prevous bar. Red text represents the percent changed is less than the FlatRange. Green text represents the percent changed is more than the FlatRange. White text represents the percent changed is within the FlatRange.

- ***Cumulative Delta:*** Displays the cumulative delta for the previous bar to the MaxBarLookBack. 
- ***Cumulative Max Delta:*** Displays the cumulative max delta for the previous bar to the MaxBarLookBack.
- ***Cumulative Min Delta:*** Displays the cumulative min delta for the previous bar to the MaxBarLookBack.

# OrderFlowStats Indicator
The following are the current properties for the OrderFlowStats Indicator:
- ***MaxBarLookBack:*** The x amount of previous bars used for calculations.
- ***FlatRange:*** The number used to determine the flat range. For example, entering 1 will create a range of 1 to -1. Anything between there will be considered flat.
