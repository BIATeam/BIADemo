import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';
import { BaseDto } from '../../../model/base-dto';
import { storeKey } from '../crud-item.constants';
import { CrudItemsReducer, State } from './crud-items-reducer';

export interface CrudItemsState<CrudItem extends BaseDto> {
  crudItems: State<CrudItem>;
}

export class CrudItemState<CrudItem extends BaseDto> {
  getAllCrudItems: (state: object) => CrudItem[];
  constructor(protected crudItemsReducer : CrudItemsReducer<CrudItem>)
  {
    this.getAllCrudItems = crudItemsReducer.crudItemsAdapter.getSelectors(this.getCrudItemsEntitiesState).selectAll;
  }

  /** Provide reducers with AoT-compilation compliance */
  public reducers(state: CrudItemsState<CrudItem> | undefined, action: Action) {
    return combineReducers({
      crudItems: this.crudItemsReducer.crudItemReducers
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  getCrudItemsState = createFeatureSelector<CrudItemsState<CrudItem>>(storeKey);

  getCrudItemsEntitiesState = createSelector(
    this.getCrudItemsState,
    (state) => state.crudItems
  );

  getCrudItemsTotalCount = createSelector(
    this.getCrudItemsEntitiesState,
    (state) => state.totalCount
  );

  getCurrentCrudItem = createSelector(
    this.getCrudItemsEntitiesState,
    (state) => state.currentCrudItem
  );

  getLastLazyLoadEvent = createSelector(
    this.getCrudItemsEntitiesState,
    (state) => state.lastLazyLoadEvent
  );

  getCrudItemLoadingGet = createSelector(
    this.getCrudItemsEntitiesState,
    (state) => state.loadingGet
  );

  getCrudItemLoadingGetAll = createSelector(
    this.getCrudItemsEntitiesState,
    (state) => state.loadingGetAll
  );
  
  getCrudItemById = (id: number) =>
    createSelector(
      this.getCrudItemsEntitiesState,
      this.crudItemsReducer.getCrudItemById(id)
    );
}