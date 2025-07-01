import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import {
  DynamicLayoutComponent,
  LayoutMode,
} from 'src/app/shared/bia-shared/components/layout/dynamic-layout/dynamic-layout.component';
import { Permission } from 'src/app/shared/permission';

import { siteCRUDConfiguration } from './site.constants';
import { FeatureSitesStore } from './store/site.state';
import { SitesEffects } from './store/sites-effects';
import { SiteEditComponent } from './views/site-edit/site-edit.component';
import { SiteItemComponent } from './views/site-item/site-item.component';
import { SiteNewComponent } from './views/site-new/site-new.component';
import { SitesIndexComponent } from './views/sites-index/sites-index.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Site_List_Access,
      injectComponent: SitesIndexComponent,
      configuration: siteCRUDConfiguration,
    },
    component: DynamicLayoutComponent,
    canActivate: [PermissionGuard],
    // [Calc] : The children are not used in calc
    children: [
      {
        path: 'create',
        data: {
          breadcrumb: 'bia.add',
          canNavigate: false,
          permission: Permission.Site_Create,
          title: 'site.add',
        },
        component: SiteNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
        },
        component: SiteItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.Site_Update,
              title: 'site.edit',
            },
            component: SiteEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin SiteModuleChildPath
          // BIAToolKit - End SiteModuleChildPath
          // Customization for teams
          {
            path: 'members',
            data: {
              breadcrumb: 'app.members',
              canNavigate: true,
              permission: Permission.Site_Member_List_Access,
              layoutMode: LayoutMode.fullPage,
            },
            loadChildren: () =>
              import('./children/members/site-member.module').then(
                m => m.SiteMemberModule
              ),
          },
        ],
      },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(
      siteCRUDConfiguration.storeKey,
      FeatureSitesStore.reducers
    ),
    EffectsModule.forFeature([SitesEffects]),
  ],
})
export class SiteModule {}
