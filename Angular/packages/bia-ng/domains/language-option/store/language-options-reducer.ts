import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { DomainLanguageOptionsActions } from './language-options-actions';

// This adapter will allow is to manipulate languages (mostly CRUD operations)
export const languageOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (language: OptionDto) => language.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Language> {
//   ids: string[] | number[];
//   entities: { [id: string]: Language };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export type LanguageOptionState = EntityState<OptionDto>;

export const INIT_LANGUAGEOPTION_STATE: LanguageOptionState =
  languageOptionsAdapter.getInitialState({
    // additional props default values here
  });

export const languageOptionReducers = createReducer<LanguageOptionState>(
  INIT_LANGUAGEOPTION_STATE,
  on(DomainLanguageOptionsActions.loadAllSuccess, (state, { languages }) =>
    languageOptionsAdapter.setAll(languages, state)
  )
  // on(loadSuccess, (state, { language }) => languageOptionsAdapter.upsertOne(language, state))
);

export const getLanguageOptionById =
  (id: number) => (state: LanguageOptionState) =>
    state.entities[id];
