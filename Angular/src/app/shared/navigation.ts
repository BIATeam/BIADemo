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
    permissions: [Permission.Plane_List_Access],
    children: [
      {
        labelKey: 'app.planes',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes']
      },
      {
        labelKey: 'app.planesPopupMode',
        permissions: [Permission.Plane_List_Access],
        path: ['/examples/planes-popup']
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
    ]
  },
  {
    labelKey: 'app.administration',
    permissions: [Permission.Airport_List_Access],
    children: [
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
    ]
  }
  // End BIADemo
];
