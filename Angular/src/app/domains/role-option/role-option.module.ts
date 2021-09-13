import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/role-option.state';
import { RoleOptionsEffects } from './store/role-options-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-roles', reducers),
    EffectsModule.forFeature([RoleOptionsEffects]),
  ]
})
export class RoleOptionModule {}


















