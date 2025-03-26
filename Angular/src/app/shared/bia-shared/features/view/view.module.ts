import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/view.state';
import { ViewsEffects } from './store/views-effects';

@NgModule({
  imports: [
    StoreModule.forFeature('views', reducers),
    EffectsModule.forFeature([ViewsEffects]),
  ],
})
export class ViewModule {}
