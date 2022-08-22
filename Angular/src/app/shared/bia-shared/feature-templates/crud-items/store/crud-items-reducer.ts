import { EntityState, createEntityAdapter, EntityAdapter } from '@ngrx/entity';
import { Action, ActionReducer, createReducer, on } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { BaseDto } from '../../../model/base-dto';
import { CrudItemsActions } from './crud-items-actions';


export interface State<CrudItem extends BaseDto> extends EntityState<CrudItem> {
  // additional props here
  totalCount: number;
  currentCrudItem: CrudItem;
  lastLazyLoadEvent: LazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export class CrudItemsReducer<CrudItem extends BaseDto> {
  public crudItemReducers: ActionReducer<State<CrudItem>, Action>;
  public crudItemsAdapter: EntityAdapter<CrudItem>;

  constructor(protected actions : CrudItemsActions<CrudItem>)
  {

    // This adapter will allow is to manipulate crudItems (mostly CRUD operations)
    this.crudItemsAdapter = createEntityAdapter<CrudItem>({
      selectId: (crudItem: CrudItem) => crudItem.id,
      sortComparer: false
    });

    // -----------------------------------------
    // The shape of EntityState
    // ------------------------------------------
    // interface EntityState<CrudItem> {
    //   ids: string[] | number[];
    //   entities: { [id: string]: CrudItem };
    // }
    // -----------------------------------------
    // -> ids arrays allow us to sort data easily
    // -> entities map allows us to access the data quickly without iterating/filtering though an array of objects


    let INIT_STATE: State<CrudItem> = this.crudItemsAdapter.getInitialState({
      // additional props default values here
      totalCount: 0,
      currentCrudItem: <CrudItem>{},
      lastLazyLoadEvent: <LazyLoadEvent>{},
      loadingGet: false,
      loadingGetAll: false,
    });

    this.crudItemReducers = createReducer<State<CrudItem>>(
      INIT_STATE,
      on(actions.loadAllByPost, (state, { event }) => {
        return { ...state, loadingGetAll: true };
      }),
      on(actions.load, (state) => {
        return { ...state, loadingGet: true };
      }),
      on(actions.loadAllByPostSuccess, (state, { result, event }) => {
        const stateUpdated = this.crudItemsAdapter.setAll(result.data, state);
        stateUpdated.totalCount = result.totalCount;
        stateUpdated.lastLazyLoadEvent = event;
        stateUpdated.loadingGetAll = false;
        return stateUpdated;
      }),
      on(actions.loadSuccess, (state, { crudItem }) => {
        return { ...state, currentCrudItem: crudItem, loadingGet: false };
      }),
      on(actions.failure, (state, { error }) => {
        return { ...state, loadingGetAll: false, loadingGet: false };
      }),
    );
  }

  public getCrudItemById = (id: number) => (state: State<CrudItem>) => state.entities[id];
}