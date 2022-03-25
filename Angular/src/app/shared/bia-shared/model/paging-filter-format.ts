import { LazyLoadEvent } from 'primeng/api';

export interface PagingFilterFormatDto extends LazyLoadEvent {
  parentIds?: number[];
  columns?: { [key: string]: string };
}
