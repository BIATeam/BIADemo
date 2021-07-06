import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HOME_ROUTES } from './features/home/home.module';
import { LayoutComponent } from './shared/bia-shared/components/layout/layout.component';
import { PageLayoutComponent } from './shared/bia-shared/components/layout/page-layout.component';

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
          // Begin BIADemo
          {
            path: 'examples',
            data: {
              breadcrumb: 'app.examples',
              canNavigate: false
            },
            children: [
              {
                path: 'planes',
                data: {
                  breadcrumb: 'app.planes',
                  canNavigate: true
                },
                loadChildren: () => import('./features/planes/plane.module').then((m) => m.PlaneModule)
              },
              {
                path: 'planes-popup',
                data: {
                  breadcrumb: 'app.planes',
                  canNavigate: true
                },
                loadChildren: () => import('./features/planes-popup/plane.module').then((m) => m.PlaneModule)
              },
              {
                path: 'planes-page',
                data: {
                  breadcrumb: 'app.planes',
                  canNavigate: true
                },
                loadChildren: () => import('./features/planes-page/plane.module').then((m) => m.PlaneModule)
              },
              {
                path: 'planes-view',
                data: {
                  breadcrumb: 'app.planes',
                  canNavigate: true,
                  // noMargin: true // Add noMargin if you wish the content of a route to stick to the borders of the screen.
                },
                loadChildren: () => import('./features/planes-view/plane.module').then((m) => m.PlaneModule)
              },
              {
                path: 'planes-signalR',
                data: {
                  breadcrumb: 'app.planes',
                  canNavigate: true
                },
                loadChildren: () => import('./features/planes-signalR/plane.module').then((m) => m.PlaneModule)
              },
              {
                path: 'planes-calc',
                data: {
                  breadcrumb: 'app.planes',
                  canNavigate: true
                },
                loadChildren: () => import('./features/planes-calc/plane.module').then((m) => m.PlaneModule)
              },
              {
                path: 'airports',
                data: {
                  breadcrumb: 'app.airports',
                  canNavigate: true
                },
                loadChildren: () => import('./features/airports/airport.module').then((m) => m.AirportModule)
              },
              {
                path: 'planes-types',
                data: {
                  breadcrumb: 'app.planesTypes',
                  canNavigate: true
                },
                loadChildren: () => import('./features/planes-types/plane-type.module').then((m) => m.PlaneTypeModule)
              },
            ]
          },
          // End BIADemo
          {
            path: 'sites',
            data: {
              breadcrumb: 'app.sites',
              canNavigate: true
            },
            loadChildren: () => import('./features/sites/site.module').then((m) => m.SiteModule)
          },
          {
            path: 'users',
            data: {
              breadcrumb: 'app.users',
              canNavigate: true
            },
            loadChildren: () => import('./features/users/user.module').then((m) => m.UserModule)
          }
        ]
      }
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
