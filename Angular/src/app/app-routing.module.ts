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
          // BIAToolKit - End RoutingDomain

          // Begin BIAToolKit Generation Ignore
          // BIAToolKit - Begin Partial RoutingDomain Fleet
          {
            path: 'fleet',
            data: {
              breadcrumb: 'app.fleet',
              canNavigate: false,
            },
            children: [
              // BIAToolKit - Begin Routing Fleet
              // BIAToolKit - Begin Partial Routing Fleet Plane
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
              // BIAToolKit - End Partial Routing Fleet Plane
              // BIAToolKit - Begin Partial Routing Fleet AircraftMaintenanceCompany
              {
                path: 'aircraft-maintenance-companies',
                data: {
                  breadcrumb: 'app.aircraftMaintenanceCompanies',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import(
                    './features/aircraft-maintenance-companies/aircraft-maintenance-company.module'
                  ).then(m => m.AircraftMaintenanceCompanyModule),
              },
              // BIAToolKit - End Partial Routing Fleet AircraftMaintenanceCompany
              // BIAToolKit - Begin Partial Routing Fleet MaintenanceTeam
              {
                path: 'maintenance-teams',
                data: {
                  breadcrumb: 'app.maintenanceTeams',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import(
                    './features/aircraft-maintenance-companies/children/maintenance-teams/maintenance-team.module'
                  ).then(m => m.MaintenanceTeamModule),
              },
              // BIAToolKit - End Partial Fleet Routing MaintenanceTeam
              // BIAToolKit - Begin Partial Fleet Routing Pilot
              {
                path: 'pilots',
                data: {
                  breadcrumb: 'app.pilots',
                  canNavigate: true,
                },
                loadChildren: () =>
                  import('./features/pilots/pilot.module').then(
                    m => m.PilotModule
                  ),
              },
              // BIAToolKit - End Partial Fleet Routing Pilot
              // BIAToolKit - Begin Partial Fleet Routing Flight
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
              // BIAToolKit - End Partial Routing Fleet Flight
              // BIAToolKit - End Routing Fleet
            ],
          },
          // BIAToolKit - End Partial RoutingDomain Fleet
          // End BIAToolKit Generation Ignore

          // Begin BIADemo
          {
            path: 'maintenance-contracts',
            data: {
              breadcrumb: 'app.maintenanceContracts',
              canNavigate: true,
            },
            loadChildren: () =>
              import(
                './features/maintenance-contracts/maintenance-contract.module'
              ).then(m => m.MaintenanceContractModule),
          },
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
              import(
                './features/bia-features/notifications/notification.module'
              ).then(m => m.NotificationModule),
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
              import(
                './features/bia-features/background-task/background-task.module'
              ).then(m => m.BackgroundTaskModule),
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
