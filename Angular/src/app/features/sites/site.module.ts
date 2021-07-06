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
import { UserModule } from 'src/app/domains/user/user.module';
import { RoleModule } from 'src/app/domains/role/role.module';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';
import { UserFromADModule } from 'src/app/domains/user-from-AD/user-from-AD.module';
import { SiteTableHeaderComponent } from './components/site-table-header/site-table-header.component';
import { SiteItemComponent } from './views/site-item/site-item.component';

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
          permission: Permission.Member_List_Access
        },
        loadChildren: () =>
          import('./children/members/member.module').then((m) => m.MemberModule)
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
    SiteTableHeaderComponent
  ],
  entryComponents: [SiteEditDialogComponent, SiteNewDialogComponent],
  imports: [
    SharedModule,
    RoleModule,
    UserModule,
    UserFromADModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('sites', reducers),
    EffectsModule.forFeature([SitesEffects]),
  ]
})
export class SiteModule { }
