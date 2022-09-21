import { LazyLoadEvent } from 'primeng/api';

export interface PagingFilterFormatDto extends LazyLoadEvent {
  parentIds?: string[];
  advancedFilter?: any;
  columns?: { [key: string]: string };
}
