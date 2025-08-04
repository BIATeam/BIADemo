import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'packages/bia-ng/core/public-api';
import { DynamicLayoutComponent } from 'packages/bia-ng/shared/public-api';
import { AircraftMaintenanceCompanyOptionModule } from 'src/app/domains/aircraft-maintenance-company-option/aircraft-maintenance-company-option.module';
import { PlaneOptionModule } from 'src/app/domains/plane-option/plane-option.module';
import { SiteOptionModule } from 'src/app/domains/site-option/site-option.module';
import { Permission } from 'src/app/shared/permission';
import { maintenanceContractCRUDConfiguration } from './maintenance-contract.constants';
import { FeatureMaintenanceContractsStore } from './store/maintenance-contract.state';
import { MaintenanceContractsEffects } from './store/maintenance-contracts-effects';
import { MaintenanceContractEditComponent } from './views/maintenance-contract-edit/maintenance-contract-edit.component';
import { MaintenanceContractImportComponent } from './views/maintenance-contract-import/maintenance-contract-import.component';
import { MaintenanceContractItemComponent } from './views/maintenance-contract-item/maintenance-contract-item.component';
import { MaintenanceContractNewComponent } from './views/maintenance-contract-new/maintenance-contract-new.component';
import { MaintenanceContractsIndexComponent } from './views/maintenance-contracts-index/maintenance-contracts-index.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.MaintenanceContract_List_Access,
      injectComponent: MaintenanceContractsIndexComponent,
      configuration: maintenanceContractCRUDConfiguration,
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
          permission: Permission.MaintenanceContract_Create,
          title: 'maintenanceContract.add',
        },
        component: MaintenanceContractNewComponent,
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
        },
        component: MaintenanceContractImportComponent,
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
            },
            component: MaintenanceContractEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin MaintenanceContractModuleChildPath
          // BIAToolKit - End MaintenanceContractModuleChildPath
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
      maintenanceContractCRUDConfiguration.storeKey,
      FeatureMaintenanceContractsStore.reducers
    ),
    EffectsModule.forFeature([MaintenanceContractsEffects]),
    // TODO after creation of CRUD MaintenanceContract : select the optioDto dommain module required for link
    // Domain Modules:
    AircraftMaintenanceCompanyOptionModule,
    PlaneOptionModule,
    SiteOptionModule,
  ],
})
export class MaintenanceContractModule {}
