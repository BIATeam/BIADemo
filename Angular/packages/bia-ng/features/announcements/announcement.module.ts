import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BiaPermission, PermissionGuard } from '@bia-team/bia-ng/core';
import { AnnouncementTypeOptionModule } from '@bia-team/bia-ng/domains';
import { DynamicLayoutComponent, LayoutMode } from '@bia-team/bia-ng/shared';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { announcementCRUDConfiguration } from './announcement.constants';
import { FeatureAnnouncementsStore } from './store/announcement.state';
import { AnnouncementsEffects } from './store/announcements-effects';
import { AnnouncementEditComponent } from './views/announcement-edit/announcement-edit.component';
import { AnnouncementHistoricalComponent } from './views/announcement-historical/announcement-historical.component';
import { AnnouncementItemComponent } from './views/announcement-item/announcement-item.component';
import { AnnouncementNewComponent } from './views/announcement-new/announcement-new.component';
import { AnnouncementsIndexComponent } from './views/announcements-index/announcements-index.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: BiaPermission.Announcement_List_Access,
      injectComponent: AnnouncementsIndexComponent,
      configuration: announcementCRUDConfiguration,
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
          permission: BiaPermission.Announcement_Create,
          title: 'announcement.add',
          style: {
            minWidth: '60vw',
            maxWidth: '60vw',
          },
        },
        component: AnnouncementNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
        },
        component: AnnouncementItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: BiaPermission.Announcement_Update,
              title: 'announcement.edit',
              style: {
                minWidth: '60vw',
                maxWidth: '60vw',
              },
            },
            component: AnnouncementEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: 'historical',
            data: {
              breadcrumb: 'bia.historical',
              canNavigate: false,
              layoutMode: LayoutMode.popup,
              style: {
                minWidth: '50vw',
              },
              title: 'bia.historical',
              permission: BiaPermission.Announcement_Read,
            },
            component: AnnouncementHistoricalComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin AnnouncementModuleChildPath
          // BIAToolKit - End AnnouncementModuleChildPath
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
      announcementCRUDConfiguration.storeKey,
      FeatureAnnouncementsStore.reducers
    ),
    EffectsModule.forFeature([AnnouncementsEffects]),
    AnnouncementTypeOptionModule,
  ],
})
export class BiaAnnouncementModule {}
