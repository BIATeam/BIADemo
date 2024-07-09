import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';
import { HangfireIndexComponent } from './views/hangfire-index/hangfire-index.component';
import { reducers } from './store/hangfire.state';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { HangfireEffects } from './store/hangfire-effects';

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
  declarations: [HangfireIndexComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('hangfire', reducers),
    EffectsModule.forFeature([HangfireEffects]),
  ],
  providers: [],
})
export class HangfireModule {}
