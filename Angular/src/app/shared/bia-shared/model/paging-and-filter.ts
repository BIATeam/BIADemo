import { LazyLoadEvent } from 'primeng/api';

export interface PagingAndFilterDto extends LazyLoadEvent {
  parentIds?: string[];
  columns?: { [key: string]: string };
}
