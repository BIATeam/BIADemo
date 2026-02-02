import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {
  LayoutComponent,
  PageLayoutComponent,
} from 'packages/bia-ng/shared/public-api';
import { HOME_ROUTES } from './features/home/home.module';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      ...HOME_ROUTES,
      {
        path: '',
        component: PageLayoutComponent,
        children: [
          // BIAToolKit - Begin RoutingDomain
          // Begin BIAToolKit Generation Ignore
          // BIAToolKit - Begin Partial RoutingDomain Maintenance
          {
            path: 'maintenance',
            data: {
              canNavigate: false,
            },
            children: [
              // BIAToolKit - Begin RoutingDomainMaintenanceChildren
              // BIAToolKit - Begin Partial RoutingDomainMaintenanceChildren AircraftMaintenanceCompany
              {
                path: 'aircraft-maintenance-companies',
                data: {
                  breadcrumb: 'app.aircraftMaintenanceCompanies',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import('./features/aircraft-maintenance-companies/aircraft-maintenance-company.module').then(
                    m => m.AircraftMaintenanceCompanyModule
                  ),
              },
              // BIAToolKit - End Partial RoutingDomainMaintenanceChildren AircraftMaintenanceCompany
              // BIAToolKit - Begin Partial RoutingDomainMaintenanceChildren MaintenanceTeam
              {
                path: 'maintenance-teams',
                data: {
                  breadcrumb: 'app.maintenanceTeams',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import('./features/aircraft-maintenance-companies/children/maintenance-teams/maintenance-team.module').then(
                    m => m.MaintenanceTeamModule
                  ),
              },
              // BIAToolKit - End Partial RoutingDomainMaintenanceChildren MaintenanceTeam
              // BIAToolKit - End RoutingDomainMaintenanceChildren
              // Begin BIADemo
              {
                path: 'maintenance-contracts',
                data: {
                  breadcrumb: 'app.maintenanceContracts',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import('./features/maintenance-contracts/maintenance-contract.module').then(
                    m => m.MaintenanceContractModule
                  ),
              },
              // End BIADemo
            ],
          },
          // BIAToolKit - End Partial RoutingDomain Maintenance
          // BIAToolKit - Begin Partial RoutingDomain Fleet
          {
            path: 'fleet',
            data: {
              canNavigate: false,
            },
            children: [
              // BIAToolKit - Begin RoutingDomainFleetChildren
              // BIAToolKit - Begin Partial RoutingDomainFleetChildren Plane
              {
                path: 'planes',
                data: {
                  breadcrumb: 'app.planes',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import('./features/planes/plane.module').then(
                    m => m.PlaneModule
                  ),
              },
              // BIAToolKit - End Partial RoutingDomainFleetChildren Plane
              // BIAToolKit - End RoutingDomainFleetChildren
              // Begin BIADemo
              {
                path: 'planes-full-code',
                data: {
                  breadcrumb: 'app.planesFullCode',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import('./features/planes-full-code/plane.module').then(
                    m => m.PlaneModule
                  ),
              },
              {
                path: 'planes-specific',
                data: {
                  breadcrumb: 'app.planesSpecific',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import('./features/planes-specific/plane.module').then(
                    m => m.PlaneModule
                  ),
              },
              {
                path: 'airports',
                data: {
                  breadcrumb: 'app.airports',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import('./features/airports/airport.module').then(
                    m => m.AirportModule
                  ),
              },
              {
                path: 'planes-types',
                data: {
                  breadcrumb: 'app.planesTypes',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import('./features/planes-types/plane-type.module').then(
                    m => m.PlaneTypeModule
                  ),
              },
              // End BIADemo
            ],
          },
          // BIAToolKit - End Partial RoutingDomain Fleet
          // End BIAToolKit Generation Ignore
          // BIAToolKit - End RoutingDomain

          // BIAToolKit - Begin Routing
          // Begin BIAToolKit Generation Ignore
          // BIAToolKit - Begin Partial Routing Pilot
          {
            path: 'pilots',
            data: {
              breadcrumb: 'app.pilots',
              canNavigate: true,
            },
            loadChildren: () =>
              import('./features/pilots/pilot.module').then(m => m.PilotModule),
          },
          // BIAToolKit - End Partial Routing Pilot
          // BIAToolKit - Begin Partial Routing Flight
          {
            path: 'flights',
            data: {
              breadcrumb: 'app.flights',
              canNavigate: true,
            },
            loadChildren: () =>
              import('./features/flights/flight.module').then(
                m => m.FlightModule
              ),
          },
          // BIAToolKit - End Partial Routing Flight
          // End BIAToolKit Generation Ignore
          // BIAToolKit - End Routing

          // Begin BIADemo
          {
            path: 'hangfire',
            data: {
              breadcrumb: 'app.hangfire',
              canNavigate: true,
            },
            loadChildren: () =>
              import('./features/hangfire/hangfire.module').then(
                m => m.HangfireModule
              ),
          },
          // End BIADemo
          {
            path: 'sites',
            data: {
              breadcrumb: 'app.sites',
              canNavigate: true,
            },
            loadChildren: () =>
              import('./features/sites/site.module').then(m => m.SiteModule),
          },
          {
            path: 'users',
            data: {
              breadcrumb: 'app.users',
              canNavigate: true,
            },
            loadChildren: () =>
              import('./features/bia-features/users/user.module').then(
                m => m.UserModule
              ),
          },
          {
            path: 'notifications',
            data: {
              breadcrumb: 'app.notifications',
              canNavigate: true,
            },
            loadChildren: () =>
              import('./features/bia-features/notifications/notification.module').then(
                m => m.NotificationModule
              ),
          },
          {
            path: 'backgroundtask',
            data: {
              breadcrumb: 'bia.backgroundtasks',
              canNavigate: true,
              noMargin: true,
              noPadding: true,
            },
            loadChildren: () =>
              import('./features/bia-features/background-task/background-task.module').then(
                m => m.BackgroundTaskModule
              ),
          },
          {
            path: 'announcements',
            data: {
              breadcrumb: 'bia.announcements',
              canNavigate: true,
            },
            loadChildren: () =>
              import('./features/bia-features/announcements/announcement.module').then(
                m => m.AnnouncementModule
              ),
          },
        ],
      },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
