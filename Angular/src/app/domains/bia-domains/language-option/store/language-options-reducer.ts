import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { loadAllSuccess } from './language-options-actions';

// This adapter will allow is to manipulate languages (mostly CRUD operations)
export const languageOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (language: OptionDto) => language.id,
  sortComparer: false
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

export interface State extends EntityState<OptionDto> {
  // additional props here
}

export const INIT_STATE: State = languageOptionsAdapter.getInitialState({
  // additional props default values here
});

export const languageOptionReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { languages }) => languageOptionsAdapter.setAll(languages, state)),
  // on(loadSuccess, (state, { language }) => languageOptionsAdapter.upsertOne(language, state))
);

export const getLanguageOptionById = (id: number) => (state: State) => state.entities[id];


















