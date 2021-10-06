import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/permission-option.state';
import { PermissionOptionsEffects } from './store/permission-options-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-permission-options', reducers),
    EffectsModule.forFeature([PermissionOptionsEffects]),
  ],
  providers: [
  ]
})
export class PermissionOptionModule {}


















