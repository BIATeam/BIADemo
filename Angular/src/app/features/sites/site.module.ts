import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { SitesEffects } from './store/sites-effects';
import { reducers } from './store/site.state';
import { SiteFormComponent } from './components/site-form/site-form.component';
import { SiteEditDialogComponent } from './views/site-edit-dialog/site-edit-dialog.component';
import { SiteNewDialogComponent } from './views/site-new-dialog/site-new-dialog.component';
import { SitesIndexComponent } from './views/sites-index/sites-index.component';
import { SiteFilterComponent } from './components/site-filter/site-filter.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';
import { SiteItemComponent } from './views/site-item/site-item.component';
import { UserOptionModule } from 'src/app/domains/bia-domains/user-option/user-option.module';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Site_List_Access
    },
    component: SitesIndexComponent,
    canActivate: [PermissionGuard]
  },
  {
    path: ':siteId',
    data: {
      breadcrumb: '',
      canNavigate: false,
    },
    component: SiteItemComponent,
    canActivate: [PermissionGuard],
    children: [
      {
        path: 'members',
        data: {
          breadcrumb: 'app.members',
          canNavigate: true,
          permission: Permission.Site_Member_List_Access
        },
        loadChildren: () =>
          import('./children/members/site-member.module').then((m) => m.SiteMemberModule)
      },
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
    declarations: [
        SiteFormComponent,
        SiteFilterComponent,
        SiteEditDialogComponent,
        SiteNewDialogComponent,
        SitesIndexComponent,
        SiteItemComponent,
    ],
    imports: [
        SharedModule,
        UserOptionModule,
        RouterModule.forChild(ROUTES),
        StoreModule.forFeature('sites', reducers),
        EffectsModule.forFeature([SitesEffects]),
    ]
})
export class SiteModule { }
