import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';
import { BackgroundTaskAdminComponent } from './views/background-task-admin/background-task-admin.component';
import { BackgroundTaskReadOnlyComponent } from './views/background-task-read-only/background-task-read-only.component';

const ROUTES: Routes = [
  {
    path: 'admin',
    data: {
      breadcrumb: 'bia.administer',
      permission: Permission.Background_Task_Admin,
    },
    component: BackgroundTaskAdminComponent,
    canActivate: [PermissionGuard]
  },
  {
    path: 'readonly',
    data: {
      breadcrumb: 'bia.view',
      permission: Permission.Background_Task_Read_Only,
    },
    component: BackgroundTaskReadOnlyComponent,
    canActivate: [PermissionGuard]
  },
  { path: '**', redirectTo: 'readonly' }
];

@NgModule({
  declarations: [
    BackgroundTaskAdminComponent,
    BackgroundTaskReadOnlyComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
  ],
  providers: []
})
export class BackgroundTaskModule { }
