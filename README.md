# Important
***Requires lifetime NinjaTrader 8 license that contains the volumetric data.*** The files in the addons, indicators and strategies folders can simply be copied over to the respective NinjaTrader folders. Re-compile by pressing F5 in the editor and the strategy and indicator should display in their list. ***Make sure to add the indicator and enable the strategy.***

# OrderFlowStats
![alt text](/images/orderflowstats.png?raw=true "OrderFlow Stats")
Displays stats about the current orderflow. The following are the current displayed stats:
- ***Median Point of Control:*** Displays the median point of control for the previous bar to the MaxBarLookBack.
- ***Cumulative Delta:*** Displays the cumulative delta for the previous bar to the MaxBarLookBack. Displays the percent changed from the MaxBarLookBack to the prevous bar.
- ***Cumulative Max Delta:*** Displays the cumulative max delta for the previous bar to the MaxBarLookBack. Displays the percent changed from the MaxBarLookBack to the prevous bar.
- ***Cumulative Min Delta:*** Displays the cumulative min delta for the previous bar to the MaxBarLookBack. Displays the percent changed from the MaxBarLookBack to the prevous bar.

# OrderFlowStrategy
The MaxBarLookBack can be changed in the properties for the strategy to calculate x amount of previous bars.