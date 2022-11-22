import { BiaNavigation } from './bia-shared/model/bia-navigation';
import { Permission } from './permission';

export const NAVIGATION: BiaNavigation[] = [
  {
    labelKey: 'app.users',
    permissions: [Permission.User_List_Access],
    path: ['/users'],
    icon: "pi pi-users"
  },
  {
    labelKey: 'app.sites',
    permissions: [Permission.Site_List_Access],
    path: ['/sites'],
    icon: "pi pi-compass"
  },
  // Begin BIADemo
  {
    labelKey: 'app.examples',
    permissions: [Permission.Plane_List_Access,
    Permission.AircraftMaintenanceCompany_List_Access,
    Permission.Hangfire_Access],
    icon: "pi pi-folder",
    children: [
      {
        labelKey: 'app.aircraft-maintenance-companies',
        permissions: [Permission.AircraftMaintenanceCompany_List_Access],
        path: ['/examples/aircraft-maintenance-companies'],
        icon: "pi pi-building"
      },
      {
        labelKey: 'app.planes',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.planesFullCode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-full-code'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.planesSpecific',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-specific'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.hangfire',
        permissions: [Permission.Hangfire_Access],
        path: ['/examples/hangfire'],
        icon: "pi pi-book"
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
    icon: "pi pi-cog",
    children: [
      {
        labelKey: 'bia.backgroundTaskAdmin',
        permissions: [Permission.Background_Task_Admin],
        path: ['/backgroundtask/admin'],
        icon: "pi pi-calendar-plus"
      },
      {
        labelKey: 'bia.backgroundTaskReadOnly',
        permissions: [Permission.Background_Task_Read_Only],
        path: ['/backgroundtask/readonly'],
        icon: "pi pi-calendar"
      },
      // Begin BIADemo
      {
        labelKey: 'app.airports',
        permissions: [Permission.Airport_List_Access],
        path: ['/examples/airports'],
        icon: "pi pi-circle"
      },
      {
        labelKey: 'app.planesTypes',
        permissions: [Permission.PlaneType_List_Access],
        path: ['/examples/planes-types'],
        icon: "pi pi-telegram"
      }
      // End BIADemo
    ]
  },

  {
    labelKey: 'app.users',
    permissions: [Permission.User_List_Access],
    path: ['/users'],
    icon: "pi pi-users"
  },
  {
    labelKey: 'app.sites',
    permissions: [Permission.Site_List_Access],
    path: ['/sites'],
    icon: "pi pi-compass"
  },
  // Begin BIADemo
  {
    labelKey: 'app.examples',
    permissions: [Permission.Plane_List_Access,
    Permission.AircraftMaintenanceCompany_List_Access,
    Permission.Hangfire_Access],
    icon: "pi pi-folder",
    children: [
      {
        labelKey: 'app.aircraft-maintenance-companies',
        permissions: [Permission.AircraftMaintenanceCompany_List_Access],
        path: ['/examples/aircraft-maintenance-companies'],
        icon: "pi pi-building"
      },
      {
        labelKey: 'app.planes',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.planesFullCode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-full-code'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.planesSpecific',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-specific'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.hangfire',
        permissions: [Permission.Hangfire_Access],
        path: ['/examples/hangfire'],
        icon: "pi pi-book"
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
    icon: "pi pi-cog",
    children: [
      {
        labelKey: 'bia.backgroundTaskAdmin',
        permissions: [Permission.Background_Task_Admin],
        path: ['/backgroundtask/admin'],
        icon: "pi pi-calendar-plus"
      },
      {
        labelKey: 'bia.backgroundTaskReadOnly',
        permissions: [Permission.Background_Task_Read_Only],
        path: ['/backgroundtask/readonly'],
        icon: "pi pi-calendar"
      },
      // Begin BIADemo
      {
        labelKey: 'app.airports',
        permissions: [Permission.Airport_List_Access],
        path: ['/examples/airports'],
        icon: "pi pi-circle"
      },
      {
        labelKey: 'app.planesTypes',
        permissions: [Permission.PlaneType_List_Access],
        path: ['/examples/planes-types'],
        icon: "pi pi-telegram"
      }
      // End BIADemo
    ]
  },

  {
    labelKey: 'app.users',
    permissions: [Permission.User_List_Access],
    path: ['/users'],
    icon: "pi pi-users"
  },
  {
    labelKey: 'app.sites',
    permissions: [Permission.Site_List_Access],
    path: ['/sites'],
    icon: "pi pi-compass"
  },
  // Begin BIADemo
  {
    labelKey: 'app.examples',
    permissions: [Permission.Plane_List_Access,
    Permission.AircraftMaintenanceCompany_List_Access,
    Permission.Hangfire_Access],
    icon: "pi pi-folder",
    children: [
      {
        labelKey: 'app.aircraft-maintenance-companies',
        permissions: [Permission.AircraftMaintenanceCompany_List_Access],
        path: ['/examples/aircraft-maintenance-companies'],
        icon: "pi pi-building"
      },
      {
        labelKey: 'app.planes',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.planesFullCode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-full-code'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.planesSpecific',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-specific'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.hangfire',
        permissions: [Permission.Hangfire_Access],
        path: ['/examples/hangfire'],
        icon: "pi pi-book"
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
    icon: "pi pi-cog",
    children: [
      {
        labelKey: 'bia.backgroundTaskAdmin',
        permissions: [Permission.Background_Task_Admin],
        path: ['/backgroundtask/admin'],
        icon: "pi pi-calendar-plus"
      },
      {
        labelKey: 'bia.backgroundTaskReadOnly',
        permissions: [Permission.Background_Task_Read_Only],
        path: ['/backgroundtask/readonly'],
        icon: "pi pi-calendar"
      },
      // Begin BIADemo
      {
        labelKey: 'app.airports',
        permissions: [Permission.Airport_List_Access],
        path: ['/examples/airports'],
        icon: "pi pi-circle"
      },
      {
        labelKey: 'app.planesTypes',
        permissions: [Permission.PlaneType_List_Access],
        path: ['/examples/planes-types'],
        icon: "pi pi-telegram"
      }
      // End BIADemo
    ]
  },

  {
    labelKey: 'app.users',
    permissions: [Permission.User_List_Access],
    path: ['/users'],
    icon: "pi pi-users"
  },
  {
    labelKey: 'app.sites',
    permissions: [Permission.Site_List_Access],
    path: ['/sites'],
    icon: "pi pi-compass"
  },
  // Begin BIADemo
  {
    labelKey: 'app.examples',
    permissions: [Permission.Plane_List_Access,
    Permission.AircraftMaintenanceCompany_List_Access,
    Permission.Hangfire_Access],
    icon: "pi pi-folder",
    children: [
      {
        labelKey: 'app.aircraft-maintenance-companies',
        permissions: [Permission.AircraftMaintenanceCompany_List_Access],
        path: ['/examples/aircraft-maintenance-companies'],
        icon: "pi pi-building"
      },
      {
        labelKey: 'app.planes',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.planesFullCode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-full-code'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.planesSpecific',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-specific'],
        icon: "pi pi-send"
      },
      {
        labelKey: 'app.hangfire',
        permissions: [Permission.Hangfire_Access],
        path: ['/examples/hangfire'],
        icon: "pi pi-book"
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
    icon: "pi pi-cog",
    children: [
      {
        labelKey: 'bia.backgroundTaskAdmin',
        permissions: [Permission.Background_Task_Admin],
        path: ['/backgroundtask/admin'],
        icon: "pi pi-calendar-plus"
      },
      {
        labelKey: 'bia.backgroundTaskReadOnly',
        permissions: [Permission.Background_Task_Read_Only],
        path: ['/backgroundtask/readonly'],
        icon: "pi pi-calendar"
      },
      // Begin BIADemo
      {
        labelKey: 'app.airports',
        permissions: [Permission.Airport_List_Access],
        path: ['/examples/airports'],
        icon: "pi pi-circle"
      },
      {
        labelKey: 'app.planesTypes',
        permissions: [Permission.PlaneType_List_Access],
        path: ['/examples/planes-types'],
        icon: "pi pi-telegram"
      }
      // End BIADemo
    ]
  },

];


