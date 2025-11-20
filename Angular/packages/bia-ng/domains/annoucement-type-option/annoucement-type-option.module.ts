import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './annoucement-type-option.constants';
import { DomainAnnoucementTypeOptionsStore } from './store/annoucement-type-option.state';
import { AnnoucementTypeOptionsEffects } from './store/annoucement-type-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(
      storeKey,
      DomainAnnoucementTypeOptionsStore.reducers
    ),
    EffectsModule.forFeature([AnnoucementTypeOptionsEffects]),
  ],
})
export class AnnoucementTypeOptionModule {}
