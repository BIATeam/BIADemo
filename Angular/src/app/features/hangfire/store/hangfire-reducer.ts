import { createEntityAdapter } from '@ngrx/entity';
import { createReducer } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// This adapter will allow is to manipulate hangfire (mostly CRUD operations)
export const hangfireAdapter = createEntityAdapter<OptionDto>({
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Hangfire> {
//   ids: string[] | number[];
//   entities: { [id: string]: Hangfire };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State {
  // additional props here
  lastLazyLoadEvent: TableLazyLoadEvent;
  loadingGet: boolean;
}

export const INIT_STATE: State = hangfireAdapter.getInitialState({
  // additional props default values here
  lastLazyLoadEvent: <TableLazyLoadEvent>{},
  loadingGet: false,
});

export const hangfireReducers = createReducer<State>(INIT_STATE);
