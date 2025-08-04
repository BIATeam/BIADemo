import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { DomainRoleOptionsStore } from './store/role-option.state';
import { RoleOptionsEffects } from './store/role-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(
      'domain-role-options',
      DomainRoleOptionsStore.reducers
    ),
    EffectsModule.forFeature([RoleOptionsEffects]),
  ],
  providers: [],
})
export class RoleOptionModule {}
