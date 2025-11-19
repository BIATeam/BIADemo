import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'packages/bia-ng/core/public-api';
import { BannerMessageTypeOptionModule } from 'packages/bia-ng/domains/public-api';
import {
  DynamicLayoutComponent,
  LayoutMode,
} from 'packages/bia-ng/shared/public-api';
import { Permission } from 'src/app/shared/permission';
import { bannerMessageCRUDConfiguration } from './banner-message.constants';
import { FeatureBannerMessagesStore } from './store/banner-message.state';
import { BannerMessagesEffects } from './store/banner-messages-effects';
import { BannerMessageEditComponent } from './views/banner-message-edit/banner-message-edit.component';
import { BannerMessageHistoricalComponent } from './views/banner-message-historical/banner-message-historical.component';
import { BannerMessageItemComponent } from './views/banner-message-item/banner-message-item.component';
import { BannerMessageNewComponent } from './views/banner-message-new/banner-message-new.component';
import { BannerMessagesIndexComponent } from './views/banner-messages-index/banner-messages-index.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.BannerMessage_List_Access,
      injectComponent: BannerMessagesIndexComponent,
      configuration: bannerMessageCRUDConfiguration,
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
          permission: Permission.BannerMessage_Create,
          title: 'bannerMessage.add',
          style: {
            minWidth: '60vw',
            maxWidth: '60vw',
          },
        },
        component: BannerMessageNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
        },
        component: BannerMessageItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.BannerMessage_Update,
              title: 'bannerMessage.edit',
              style: {
                minWidth: '60vw',
                maxWidth: '60vw',
              },
            },
            component: BannerMessageEditComponent,
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
              permission: Permission.BannerMessage_Read,
            },
            component: BannerMessageHistoricalComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin BannerMessageModuleChildPath
          // BIAToolKit - End BannerMessageModuleChildPath
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
      bannerMessageCRUDConfiguration.storeKey,
      FeatureBannerMessagesStore.reducers
    ),
    EffectsModule.forFeature([BannerMessagesEffects]),
    BannerMessageTypeOptionModule,
  ],
})
export class BiaBannerMessageModule {}
