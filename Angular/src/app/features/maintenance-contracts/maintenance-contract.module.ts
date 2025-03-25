import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { ReducerManager, StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { Permission } from 'src/app/shared/permission';
import { SharedModule } from 'src/app/shared/shared.module';
import { MaintenanceContractFormComponent } from './components/maintenance-contract-form/maintenance-contract-form.component';
import { MaintenanceContractItemComponent } from './views/maintenance-contract-item/maintenance-contract-item.component';
import { MaintenanceContractsIndexComponent } from './views/maintenance-contracts-index/maintenance-contracts-index.component';
// BIAToolKit - Begin Option AircraftMaintenanceCompany
import { AircraftMaintenanceCompanyOptionModule } from 'src/app/domains/aircraft-maintenance-company-option/aircraft-maintenance-company-option.module';
// BIAToolKit - End Option AircraftMaintenanceCompany
// BIAToolKit - Begin Option Plane
import { PlaneOptionModule } from 'src/app/domains/plane-option/plane-option.module';
// BIAToolKit - End Option Plane
// BIAToolKit - Begin Option Site
import { SiteOptionModule } from 'src/app/domains/site-option/site-option.module';
// BIAToolKit - End Option Site
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { CrudItemImportModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item-import.module';
import { CrudItemModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item.module';
import { MaintenanceContractTableComponent } from './components/maintenance-contract-table/maintenance-contract-table.component';
import { maintenanceContractCRUDConfiguration } from './maintenance-contract.constants';
import { FeatureMaintenanceContractsStore } from './store/maintenance-contract.state';
import { MaintenanceContractsEffects } from './store/maintenance-contracts-effects';
import { MaintenanceContractEditComponent } from './views/maintenance-contract-edit/maintenance-contract-edit.component';
import { MaintenanceContractImportComponent } from './views/maintenance-contract-import/maintenance-contract-import.component';
import { MaintenanceContractNewComponent } from './views/maintenance-contract-new/maintenance-contract-new.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.MaintenanceContract_List_Access,
      injectComponent: MaintenanceContractsIndexComponent,
    },
    component: FullPageLayoutComponent,
    canActivate: [PermissionGuard],
    // [Calc] : The children are not used in calc
    children: [
      {
        path: 'create',
        data: {
          breadcrumb: 'bia.add',
          canNavigate: false,
          permission: Permission.MaintenanceContract_Create,
          title: 'maintenanceContract.add',
          injectComponent: MaintenanceContractNewComponent,
          dynamicComponent: () =>
            maintenanceContractCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: maintenanceContractCRUDConfiguration.usePopup
          ? PopupLayoutComponent
          : FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'import',
        data: {
          breadcrumb: 'maintenance-contract.import',
          canNavigate: false,
          style: {
            minWidth: '80vw',
            maxWidth: '80vw',
            maxHeight: '80vh',
          },
          permission: Permission.MaintenanceContract_Save,
          title: 'maintenanceContract.import',
          injectComponent: MaintenanceContractImportComponent,
          dynamicComponent: () =>
            maintenanceContractCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: maintenanceContractCRUDConfiguration.usePopup
          ? PopupLayoutComponent
          : FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: true,
        },
        component: MaintenanceContractItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.MaintenanceContract_Update,
              title: 'maintenanceContract.edit',
              injectComponent: MaintenanceContractEditComponent,
              dynamicComponent: () =>
                maintenanceContractCRUDConfiguration.usePopup
                  ? PopupLayoutComponent
                  : FullPageLayoutComponent,
            },
            component: maintenanceContractCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin PlaneModuleChildPath
          // BIAToolKit - End PlaneModuleChildPath
        ],
      },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  declarations: [
    MaintenanceContractItemComponent,
    MaintenanceContractsIndexComponent,
    // [Calc] : NOT used for calc (3 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    MaintenanceContractFormComponent,
    MaintenanceContractNewComponent,
    MaintenanceContractEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    MaintenanceContractTableComponent,
    MaintenanceContractImportComponent,
  ],
  imports: [
    SharedModule,
    CrudItemModule,
    CrudItemImportModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(
      maintenanceContractCRUDConfiguration.storeKey,
      FeatureMaintenanceContractsStore.reducers
    ),
    EffectsModule.forFeature([MaintenanceContractsEffects]),
    // TODO after creation of CRUD MaintenanceContract : select the optioDto dommain module required for link
    // Domain Modules:
    // BIAToolKit - Begin Option AircraftMaintenanceCompany
    AircraftMaintenanceCompanyOptionModule,
    // BIAToolKit - End Option AircraftMaintenanceCompany
    // BIAToolKit - Begin Option Plane
    PlaneOptionModule,
    // BIAToolKit - End Option Plane
    // BIAToolKit - Begin Option Site
    SiteOptionModule,
    // BIAToolKit - End Option Site
  ],
})
export class MaintenanceContractModule {}
