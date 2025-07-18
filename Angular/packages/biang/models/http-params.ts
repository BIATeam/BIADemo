import { TableLazyLoadEvent } from 'primeng/table';
import { HttpOptions } from './http-options';

interface HttpParam {
  offlineMode?: boolean;
  options?: HttpOptions;
  endpoint?: string;
}

export interface GetParam extends HttpParam {
  id?: string | number;
}

export type GetListParam = HttpParam;

export interface GetListByPostParam extends HttpParam {
  event: TableLazyLoadEvent;
}

export interface SaveParam<TIn> extends HttpParam {
  items: TIn[];
}

export interface PutParam<TIn> extends HttpParam {
  item: TIn;
  id: string | number;
}

export interface PostParam<TIn> extends HttpParam {
  item: TIn;
}

export interface DeleteParam extends HttpParam {
  id: string | number;
}

export interface DeletesParam extends HttpParam {
  ids: string[] | number[];
}

export interface UpdateFixedStatusParam extends HttpParam {
  id: string | number;
  fixed: boolean;
}
