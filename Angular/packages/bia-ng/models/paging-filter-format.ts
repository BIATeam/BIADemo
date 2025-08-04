import { TableLazyLoadEvent } from 'primeng/table';

export interface PagingFilterFormatDto extends TableLazyLoadEvent {
  parentIds?: string[];
  advancedFilter?: any;
  columns?: { [key: string]: string };
}
