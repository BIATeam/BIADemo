import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { DomainLanguageOptionsStore } from './store/language-option.state';
import { LanguageOptionsEffects } from './store/language-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(
      'domain-language-options',
      DomainLanguageOptionsStore.reducers
    ),
    EffectsModule.forFeature([LanguageOptionsEffects]),
  ],
})
export class LanguageOptionModule {}
