# CRUD
This document explains how to quickly create a CRUD module feature.   
<u>For this example, we imagine that we want to create a new feature with the name: <span style="background-color:yellow">aircrafts</span>.   </u>

## Prerequisite
The back-end is ready, i.e. the <span style="background-color:yellow">Aircraft</span> controller exists as well as permissions such as `Aircraft_List_Access`.

## Create a new feature
First, create a new <span style="background-color:yellow">aircrafts</span> folder under the **src\app\features** folder of your project.   
Then copy, paste and unzip into this feature <span style="background-color:yellow">aircrafts</span> folder the contents of :
* If you want a CRUD without relation (generaly use in Administrative parameter list):
  * **Angular\docs\feature-airports.zip** 
* If you want a CRUD with relation, like option selectable (this requiered that you create the option module before), or filter by site:
  * **Angular\docs\feature-planes-popup.zip** open the edit form in a popup
  * **Angular\docs\feature-planes-page.zip** open the edit form in a full screen page
  * **Angular\docs\feature-planes-SignalR.zip** open the edit form in a popup and list is refreshed by SignalR if there is modification detected by the back.
  * **Angular\docs\feature-planes-view.zip** open the edit form in a popup and add the view system to filter the lists.
  * **Angular\docs\feature-planes-calc.zip** spreadsheet mode.

Then, inside the folder of your new feature, execute the file **new-crud.ps1**   
For **new crud name? (singular)**, type <span style="background-color:yellow">aircraft</span>   (use - in complexe name example : plane-type)
For **new crud name? (plural)**, type <span style="background-color:yellow">aircrafts</span>    (use - in complexe name example : planes-types)
When finished, you can delete **new-crud.ps1**   

## Update permission
Open the file **src\app\shared\permission.ts** and in the **Permission** enum, add
```typescript
  Aircraft_Create = 'Aircraft_Create',
  Aircraft_Delete = 'Aircraft_Delete',
  Aircraft_List_Access = 'Aircraft_List_Access',
  Aircraft_Read = 'Aircraft_Read',
  Aircraft_Save = 'Aircraft_Save',
  Aircraft_Update = 'Aircraft_Update',
```
For complexe name use : PlaneType_Create = 'PlaneType_Create',


## Update navigation
Open the file **src\app\shared\navigation.ts** and in the array **NAVIGATION**, add 
```typescript
{
  labelKey: 'app.aircrafts',
  permissions: [Permission.Aircraft_List_Access],
  path: ['/aircrafts']
}
```
example for complexe name:
```typescript
{
  labelKey: 'app.planesTypes',
  permissions: [Permission.PlaneType_List_Access],
  path: ['/examples/planes-types']
}
```

## Update routing
Open the file **src\app\app-routing.module.ts** and in the array **routes**, add 
```typescript
{
  path: 'aircrafts',
  data: {
    breadcrumb: 'app.aircrafts',
    canNavigate: true
    },
    loadChildren: () => import('./features/aircrafts/aircraft.module').then((m) => m.AircraftModule)
}
```
example for complexe name:
```typescript
{
  path: 'planes-types',
  data: {
    breadcrumb: 'app.planesTypes',
    canNavigate: true
  },
  loadChildren: () => import('./features/planes-types/plane-type.module').then((m) => m.PlaneTypeModule)
}
```

## Create the model
Use the back-end with swagger to retrieve a json from the new entity.   
Use this site to convert the json to interface TypeScript:   
http://json2ts.com/   
And then, copy the generated code in **src\app\features\aircrafts\model\aircraft.ts**

If you have a related model you have 2 models to generate :
- one for the item display in the list <span style="background-color:yellow">Aircraft</span>ListItem (based on the swagger result of /api/<span style="background-color:yellow">Aircrafts</span>/all)
- one for the item display in the list <span style="background-color:yellow">Aircraft</span> (based on the swagger result of /api/<span style="background-color:yellow">Aircrafts</span>/{id}  and replace all any and object generated (except Site) by OptionDto)


## Update translations
Open the file **src\assets\i18n\app\en.json** and   
add in `"app"` (use camel case : "planesTypes": "Planes types",)
```json
"app": {
    ...
    "aircrafts": "Aircrafts"
  }
```
add (use Camel case in the json labe ex: "planeType": { )
```json
"aircraft": {
  "add": "Add aircraft",
  "edit": "Edit aircraft",
  "listOf": "List of aircrafts"
  }
```
and add translations of interface properties.

Open the file **src\assets\i18n\app\fr.json** and
add in `"app"`
```json
"app": {
    ...
    "aircrafts": "Aéronefs"
  }
```
add
```json
"aircraft": {
    "add": "Ajouter aéronef",
    "edit": "Modifier aéronef",
    "listOf": "Liste des aéronefs"
  }
```
and add translations of interface properties.

Open the file **src\assets\i18n\app\es.json** and
add in `"app"`
```json
"app": {
    ...
    "aircrafts": "Aeronaves"
  }
```
add
```json
"aircraft": {
   "add": "Añadir aeronave",
    "edit": "Editar aeronave",
    "listOf": "Lista de aeronaves"
  }
```
and add translations of interface properties.

When you have finished adding translations, use this site to sort your json:
https://novicelab.org/jsonabc/

## Form
Change the following form component to match the business requirements:
**src\app\features\aircrafts\components\aircraft-form**

## Table
Open the following file **src\app\features\aircrafts\views\aircrafts-index\aircrafts-index.component.ts**   
Change columns field of the `tableConfiguration` variable according to the columns in the table you want to display.   
If it is a simple string type column with filter and possible sorting, then use this column line to define your column.
```typescript
new PrimeTableColumn('msn', 'aircraft.msn'),
```
If the column type is not a string, or if the column is not sortable, or not sortable, you must define the column as follows: 
```typescript
Object.assign(new PrimeTableColumn('msn', 'aircraft.msn'), {
        isSearchable: false,
        isSortable: false,
        type: TypeTS.Number
      })
```

## Relation to Option
Adapt (or supress if not use) the differente relation to Option:
In ngOnInit function of the two files:
* **src\app\features\aircrafts\views\aircrafts-edit-dialog\aircrafts-edit-dialog.component.ts**
* **src\app\features\aircrafts\views\aircrafts-new-dialog\aircrafts-new-dialog.component.ts**
For example, add the following lines two have the option list synchronized (for the type : PlaneType):
```typescript
    this.planeTypeOptions$ = this.store.select(getAllPlaneTypeOptions).pipe();
    this.store.dispatch(loadAllPlaneTypeOptions());
```

## Enable Views

To enable views on this feature, set the **aircrafts-index.component.ts** file.  
Add this import:
```typescript
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
```
in `ngOnInit`, add:
```typescript
ngOnInit() {
  ...
  this.store.dispatch(loadAllView());
}
```
add this property:
```typescript
tableStateKey = 'aircraftsGrid';
```
Set the **aircrafts-index.component.html** file.  
Add the input parameter `tableStateKey` for the `bia-table-controller` and `bia-table` like this:

```html
<bia-table-controller
...
[tableStateKey]="tableStateKey"
></bia-table-controller>
```

```html
<bia-table
...
[tableStateKey]="tableStateKey"
></bia-table>
```

## Enable SignalR:
* Use the **Angular\docs\feature-planes-SignalR.zip** and the new_crud.ps1 to generate the files at the right name.
  Copy the <span style="background-color:yellow">aircraft</span>-signalR.service.ts in your features/<span style="background-color:yellow">aircrafts</span>/services folder
* In the <span style="background-color:yellow">aircraft</span>-index.component.ts constructor add (replace aircraft by your feature name):
  ```typescript
    constructor(
        private store: Store<AppState>,
        private authService: AuthService,
        private planeDas: PlaneDas,
        private translateService: TranslateService,
        private biaTranslationService: BiaTranslationService,
        private aircraftsSignalRService: PlanesSignalRService
      ) {
        this.aircraftsSignalRService.initialize();
      }
  ```
* In <span style="background-color:yellow">aircraft</span>.module.ts add the provider after import:
  ```typescript
    imports: [
        ...
    ],
    providers: [
      AircraftsSignalRService
    ]
  ```
* In the <span style="background-color:yellow">aircrafts</span>-effects.ts modify create, update, remove, multiRemove to not more use return loadAllByPost but use biaSuccessWaitRefreshSignalR() :
  ```typescript
    create$ = createEffect(() =>
      this.actions$.pipe(
        ofType(create),
        pluck('aircraft'),
        concatMap((aircraft) => of(aircraft).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
        switchMap(([aircraft, event]) => {
          return this.aircraftDas.post(aircraft).pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              // Uncomment this if you do not use SignalR to refresh
              // return loadAllByPost({ event: <LazyLoadEvent>event });
              // Uncomment this if you use SignalR to refresh
              return biaSuccessWaitRefreshSignalR();
            }),
            catchError((err) => {
              this.biaMessageService.showError();
              return of(failure({ error: err }));
            })
          );
        })
      )
    );
  ```

## Spreadsheet mode
### OneToMany / ManyToMany
In your view **index.component.ts**, you must define the property **type**
```typescript
  private initTableConfiguration() {
   ...
          Object.assign(new PrimeTableColumn('planeType', 'plane.planeType'), {
            type: PropType.OneToMany
          }),
          Object.assign(new PrimeTableColumn('connectingAirports', 'plane.connectingAirports'), {
            type: PropType.ManyToMany
          })

```

You must then pass your optionDtos via the observable **dictOptionDtos$**. You must respect the order of the observables in **combineLatest** and the order of creation of **DictOptionDto**

```typescript
ngOnInit() {
  ...
this.dictOptionDtos$ = combineLatest([planeTypeOptions$, airportOptions$]).pipe(
      map(
        (options) =>
          <DictOptionDto[]>[
            new DictOptionDto('planeType', options[0]),
            new DictOptionDto('connectingAirports', options[1])
          ]
      )
    );
```
Don't forget to pass the **dictOptionDtos$** observable in your html 
```html
<app-plane-table
      [elements]="planes$ | async"
      [dictOptionDtos]="dictOptionDtos$ | async"
```

### Specific Input
For specific properties that are not managed by the Framework, your table component must have an html. In this html, you have to copy/paste the content of the html from **bia-calc-table.component.html**.

In this html, you must add your components in the **SPECIFIC INPUT** zone and **SPECIFIC OUTPUT** zone.

```html
<!-- Begin Add here specific input -->
<!-- End Add here specific input -->
...
<!-- Begin Add here specific output -->
<!-- End Add here specific output -->
```

in your typescript file, you must fill the **specificInputs** property.

``` typescript
specificInputs: string[] = [
    'potting',
    'locking',
    ...
  ];
```
