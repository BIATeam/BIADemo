import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BiaPermission, PermissionGuard } from '@bia-team/bia-ng/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';

import { HangfireEffects } from './store/hangfire-effects';
import { reducers } from './store/hangfire.state';
import { HangfireIndexComponent } from './views/hangfire-index/hangfire-index.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: BiaPermission.Notification_List_Access,
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
  ],
})
export class HangfireModule {}
