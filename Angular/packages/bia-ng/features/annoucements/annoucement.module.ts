import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'packages/bia-ng/core/public-api';
import { AnnoucementTypeOptionModule } from 'packages/bia-ng/domains/public-api';
import {
  DynamicLayoutComponent,
  LayoutMode,
} from 'packages/bia-ng/shared/public-api';
import { Permission } from 'src/app/shared/permission';
import { annoucementCRUDConfiguration } from './annoucement.constants';
import { FeatureAnnoucementsStore } from './store/annoucement.state';
import { AnnoucementsEffects } from './store/annoucements-effects';
import { AnnoucementEditComponent } from './views/annoucement-edit/annoucement-edit.component';
import { AnnoucementHistoricalComponent } from './views/annoucement-historical/annoucement-historical.component';
import { AnnoucementItemComponent } from './views/annoucement-item/annoucement-item.component';
import { AnnoucementNewComponent } from './views/annoucement-new/annoucement-new.component';
import { AnnoucementsIndexComponent } from './views/annoucements-index/annoucements-index.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Annoucement_List_Access,
      injectComponent: AnnoucementsIndexComponent,
      configuration: annoucementCRUDConfiguration,
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
          permission: Permission.Annoucement_Create,
          title: 'annoucement.add',
          style: {
            minWidth: '60vw',
            maxWidth: '60vw',
          },
        },
        component: AnnoucementNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
        },
        component: AnnoucementItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.Annoucement_Update,
              title: 'annoucement.edit',
              style: {
                minWidth: '60vw',
                maxWidth: '60vw',
              },
            },
            component: AnnoucementEditComponent,
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
              permission: Permission.Annoucement_Read,
            },
            component: AnnoucementHistoricalComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin AnnoucementModuleChildPath
          // BIAToolKit - End AnnoucementModuleChildPath
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
      annoucementCRUDConfiguration.storeKey,
      FeatureAnnoucementsStore.reducers
    ),
    EffectsModule.forFeature([AnnoucementsEffects]),
    AnnoucementTypeOptionModule,
  ],
})
export class BiaAnnoucementModule {}
