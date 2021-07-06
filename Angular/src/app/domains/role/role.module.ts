import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/role.state';
import { RolesEffects } from './store/roles-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-roles', reducers),
    EffectsModule.forFeature([RolesEffects]),
  ]
})
export class RoleModule {}
