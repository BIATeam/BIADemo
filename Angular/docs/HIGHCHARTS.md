# Highcharts details for Framework V3

This files list the details about how to use Highcharts in DM Framework V3 with Angular
**Every developer who use this component should contribute to this file**

## Documentation:
* Highchart-Angular Wrapper Doc: https://www.npmjs.com/package/highcharts-angular
* Highchart Official Doc: https://www.highcharts.com/docs/index

## Highchart Demos:
* https://www.highcharts.com/demo
* https://www.highcharts.com/demo/gantt
* https://www.highcharts.com/demo/maps
* https://www.highcharts.com/demo/stock

## Example in Project:
* ePlan (Using Highcharts with Day/Week/Month grouping, and Highcharts Gantt with drag & drop)

## When to use Highcharts:
Highcharts is best used over PrimeNG components when you need advanced chart options, like:
* Combined graphs (Lines + Stacked bars, etc...)
* Gantt Chart
* Advanced controls (Like Zooming, Exporting, Range Selector)

## Installation:
* 1- Use the npm package (Follow the instructions on the Documentation part)
* 2- Import Highcharts to the modules you want to use instructions
```import { HighchartsChartModule } from 'highcharts-angular'; ```

* 3- Add/Set up highchart as components inside your feature(s)
* 4- Link your Views to the highchart component as childs be displayed (Input and Output mecanism)

## Extra Modules:
When you import highcharts, highchart component will have vanilla/basic functions.
* If you need to add specific functions to the chart (like Drag & Drop, Exporting functions, or Range Selector) you need to import extra highchart modules.
* (See highchart documentation and modules requirement for the full details)
* Below an example using the export data function, range Selector and drag drop:

```
**example file: workcapacity-chart.component.ts**

import { Component, Input, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';
import draggablepoint from 'highcharts/modules/draggable-points';
import { ChartData } from '../../model/gantthelper';
import exporting from 'highcharts/modules/exporting';
import exportdata from 'highcharts/modules/export-data';
import accessibility from 'highcharts/modules/accessibility';
import { XAxisOptions } from 'highcharts';
import { UnitType } from '../../model/unittype';

draggablepoint(Highcharts);
exporting(Highcharts);
exportdata(Highcharts);
accessibility(Highcharts);
```