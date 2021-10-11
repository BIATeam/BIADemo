import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/user-option.state';
import { UserOptionsEffects } from './store/user-options-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-user-options', reducers),
    EffectsModule.forFeature([UserOptionsEffects]),
  ]
})
export class UserOptionModule {}


















