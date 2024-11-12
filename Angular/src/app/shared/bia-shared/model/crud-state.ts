import { TableLazyLoadEvent } from 'primeng/table';

export const DEFAULT_CRUD_STATE: <T>() => CrudState<T> = () => ({
  // additional props default values here
  totalCount: 0,
  lastLazyLoadEvent: <TableLazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
});

export interface CrudState<T> {
  // additional props here
  totalCount: number;
  currentItem?: T;
  lastLazyLoadEvent: TableLazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}
