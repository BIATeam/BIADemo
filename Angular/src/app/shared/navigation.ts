import { BiaNavigation } from './bia-shared/model/bia-navigation';
import { Permission } from './permission';

export const NAVIGATION: BiaNavigation[] = [
  {
    labelKey: 'app.users',
    permissions: [Permission.User_List_Access],
    path: ['/users']
  },
  {
    labelKey: 'app.sites',
    permissions: [Permission.Site_List_Access],
    path: ['/sites']
  },
  // Begin BIADemo
  {
    labelKey: 'app.examples',
    permissions: [Permission.Plane_List_Access, 
      Permission.AircraftMaintenanceCompany_List_Access ,
      Permission.Hangfire_Access ],
    children: [
      {
        labelKey: 'app.aircraft-maintenance-companies',
        permissions: [Permission.AircraftMaintenanceCompany_List_Access],
        path: ['/examples/aircraft-maintenance-companies']
      },
      {
        labelKey: 'app.planes',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes']
      },
      {
        labelKey: 'app.planesPageMode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-page']
      },
      {
        labelKey: 'app.planesViewMode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-view']
      },
      {
        labelKey: 'app.planesSignalRMode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-signalR']
      },
      {
        labelKey: 'app.planesCalcMode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-calc']
      },
      {
        labelKey: 'app.planesOfflineMode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-offline']
      },
      {
        labelKey: 'app.planesSpecific',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-specific']
      },
      {
        labelKey: 'app.planesLight',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-light']
      },
      {
        labelKey: 'app.hangfire',
        permissions: [Permission.Hangfire_Access],
        path: ['/examples/hangfire']
      },
    ]
  },
  // End BIADemo
  {
    labelKey: 'bia.administration',
    permissions: [
      Permission.Background_Task_Admin,
      Permission.Background_Task_Read_Only,
      // Begin BIADemo
      Permission.Airport_List_Access,
      Permission.PlaneType_List_Access,
      // End BIADemo
    ],
    children: [
      {
        labelKey: 'bia.backgroundTaskAdmin',
        permissions: [Permission.Background_Task_Admin],
        path: ['/backgroundtask/admin']
      },
      {
        labelKey: 'bia.backgroundTaskReadOnly',
        permissions: [Permission.Background_Task_Read_Only],
        path: ['/backgroundtask/readonly']
      },
      // Begin BIADemo
      {
        labelKey: 'app.airports',
        permissions: [Permission.Airport_List_Access],
        path: ['/examples/airports']
      },
      {
        labelKey: 'app.planesTypes',
        permissions: [Permission.PlaneType_List_Access],
        path: ['/examples/planes-types']
      }
      // End BIADemo
    ]
  },

];
