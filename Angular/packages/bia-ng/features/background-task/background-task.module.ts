import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {
  BiaPermission,
  PermissionGuard,
} from 'packages/bia-ng/core/public-api';
import { BackgroundTaskAdminComponent } from './views/background-task-admin/background-task-admin.component';
import { BackgroundTaskReadOnlyComponent } from './views/background-task-read-only/background-task-read-only.component';

const ROUTES: Routes = [
  {
    path: 'admin',
    data: {
      breadcrumb: 'bia.administer',
      permission: BiaPermission.Background_Task_Admin,
    },
    component: BackgroundTaskAdminComponent,
    canActivate: [PermissionGuard],
  },
  {
    path: 'readonly',
    data: {
      breadcrumb: 'bia.view',
      permission: BiaPermission.Background_Task_Read_Only,
    },
    component: BackgroundTaskReadOnlyComponent,
    canActivate: [PermissionGuard],
  },
  { path: '**', redirectTo: 'readonly' },
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  providers: [],
})
export class BiaBackgroundTaskModule {}
