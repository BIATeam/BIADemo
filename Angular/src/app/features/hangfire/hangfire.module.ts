import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';

import { HangfireEffects } from './store/hangfire-effects';
import { reducers } from './store/hangfire.state';
import { HangfireIndexComponent } from './views/hangfire-index/hangfire-index.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Notification_List_Access,
    },
    component: HangfireIndexComponent,
    canActivate: [PermissionGuard],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('hangfire', reducers),
    EffectsModule.forFeature([HangfireEffects]),
    HangfireIndexComponent,
  ],
  providers: [],
})
export class HangfireModule {}
