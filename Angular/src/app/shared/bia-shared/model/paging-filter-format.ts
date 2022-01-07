import { LazyLoadEvent } from 'primeng/api';

export interface PagingFilterFormatDto extends LazyLoadEvent {
  parentIds?: string[];
  columns?: { [key: string]: string };
}
