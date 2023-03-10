# Important
***Requires lifetime NinjaTrader 8 license that contains the volumetric data.*** The files in the addons, indicators folders can simply be copied over to the respective NinjaTrader folders. Re-compile by pressing F5 in the editor and indicator should display in their list.

# OrderFlowStats
<p align="center">
  <img src="https://raw.githubusercontent.com/walleyyang/orderflow/main/images/orderflowstats.png">
</p>

Displays stats about the current orderflow. The following are the current displayed stats:
- ***Median Point of Control:*** Displays the median point of control for the previous bar to the MaxBarLookBack. Red text represents the open price of the current bar is below the median point of control. Green text represents the open price of the current bar is above the median point of control. White represents the current bar open is the same price as the median point of control.

***Cumulative Deltas:*** 
- ***First Number Set:*** Displays the percent changed from the MaxBarLookBack to the previous bar. Red text represents the percent changed is less than the FlatRange. Green text represents the percent changed is more than the FlatRange. White text represents the percent changed is within the FlatRange.

- ***Second Number Set:*** Displays the percent changed from the previous bar's delta and the current delta. Green text represents an increase from the previous bar's delta. Red text represents a decrease from the previous bar's delta.

***Linear Regression Slope*** 
- ***First Number Set:*** Displays the highest and lowest linear regression slope for closed bars since the open of the session.

- ***Second Number Set:*** Displays the current linear regression slope. Green text represents an increase from the previous bar's linear regression slope. Red text represents a decrease from the previous bar's linear regression slope.

# OrderFlowStats Indicator
The following are the current properties for the OrderFlowStats Indicator:
- ***MaxBarLookBack:*** The x amount of previous bars used for calculations.
- ***FlatRange:*** The number in percentage used to determine the flat range. For example, entering 10 will create a range of 10 to -10. Anything between there will be considered flat.
