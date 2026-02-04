import { CrudState, DEFAULT_CRUD_STATE } from '@bia-team/bia-ng/models';
import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { MaintenanceContract } from '../model/maintenance-contract';
import { FeatureMaintenanceContractsActions } from './maintenance-contracts-actions';

// This adapter will allow is to manipulate maintenanceContracts (mostly CRUD operations)
export const maintenanceContractsAdapter =
  createEntityAdapter<MaintenanceContract>({
    selectId: (maintenanceContract: MaintenanceContract) =>
      maintenanceContract.id,
    sortComparer: false,
  });

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<MaintenanceContract> {
//   ids: string[] | number[];
//   entities: { [id: string]: MaintenanceContract };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State
  extends CrudState<MaintenanceContract>, EntityState<MaintenanceContract> {
  // additional props here
}

export const INIT_STATE: State = maintenanceContractsAdapter.getInitialState({
  ...DEFAULT_CRUD_STATE(),
  // additional props default values here
});

export const maintenanceContractReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureMaintenanceContractsActions.clearAll, state => {
    const stateUpdated = maintenanceContractsAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeatureMaintenanceContractsActions.clearCurrent, state => {
    return { ...state, currentItem: <MaintenanceContract>{} };
  }),
  on(FeatureMaintenanceContractsActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureMaintenanceContractsActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(FeatureMaintenanceContractsActions.save, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(
    FeatureMaintenanceContractsActions.loadAllByPostSuccess,
    (state, { result, event }) => {
      const stateUpdated = maintenanceContractsAdapter.setAll(
        result.data,
        state
      );
      stateUpdated.totalCount = result.totalCount;
      stateUpdated.lastLazyLoadEvent = event;
      stateUpdated.loadingGetAll = false;
      return stateUpdated;
    }
  ),
  on(
    FeatureMaintenanceContractsActions.loadSuccess,
    (state, { maintenanceContract }) => {
      return { ...state, currentItem: maintenanceContract, loadingGet: false };
    }
  ),
  on(FeatureMaintenanceContractsActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getMaintenanceContractById = (id: number) => (state: State) =>
  state.entities[id];
