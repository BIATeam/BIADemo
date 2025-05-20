import { Injectable } from '@angular/core';
import { FilterMetadata } from 'primeng/api';
import { TableLazyLoadEvent } from 'primeng/table';
import { TABLE_FILTER_GLOBAL } from '../../constants';
import { BiaLayoutService } from '../components/layout/services/layout.service';
import { BiaTableComponent } from '../components/table/bia-table/bia-table.component';
import { clone } from '../utils';
import { LayoutHelperService } from './layout-helper.service';

@Injectable({
  providedIn: 'root',
})
export class TableHelperService {
  static copyTableLazyLoadEvent(
    lazyLoadEvent: TableLazyLoadEvent
  ): TableLazyLoadEvent {
    return {
      filters: clone(lazyLoadEvent.filters),
      first: lazyLoadEvent.first,
      globalFilter: clone(lazyLoadEvent.globalFilter),
      last: lazyLoadEvent.last,
      multiSortMeta: clone(lazyLoadEvent.multiSortMeta),
      rows: lazyLoadEvent.rows,
      sortField: lazyLoadEvent.sortField,
      sortOrder: lazyLoadEvent.sortOrder,
    };
  }

  public hasFilter<TDto extends { id: number }>(
    biaTableComponent: BiaTableComponent<TDto>
  ): boolean {
    if (this.isNullUndefEmptyStr(biaTableComponent)) {
      return false;
    }
    if (biaTableComponent.table && biaTableComponent.table.hasFilter()) {
      if (this.isNullUndefEmptyFilters(biaTableComponent.table.filters, true)) {
        return false;
      } else {
        return true;
      }
    } else {
      return false;
    }
  }

  public isNullUndefEmptyFilters(
    filters: { [s: string]: FilterMetadata | FilterMetadata[] | undefined },
    ignoreGlobalFilter: boolean
  ): boolean {
    if (!filters) {
      return true;
    }
    if (this.isNullUndefEmptyStr(filters)) {
      return true;
    }
    if (JSON.stringify(filters) === '{}') {
      return true;
    }
    for (const [key, filter] of Object.entries(filters)) {
      if (ignoreGlobalFilter && key.startsWith(TABLE_FILTER_GLOBAL)) {
        continue;
      }
      if (this.isSimpleFilter(filter)) {
        // simple filter
        if (this.isEmptyFilter(<FilterMetadata>filter)) {
          continue;
        }
      } else {
        // array filter
        for (const filter2 of filter as FilterMetadata[]) {
          if (this.isEmptyFilter(filter2)) {
            continue;
          }
          return false;
        }
        continue;
      }
      return false;
    }
    return true;
  }

  public isSimpleFilter(filter: FilterMetadata | FilterMetadata[] | undefined) {
    return !Array.isArray(filter);
  }

  protected isNullUndefEmptyStr(obj: any): boolean {
    return obj === null || obj === undefined || obj === '';
  }

  public isEmptyFilter(obj: FilterMetadata): boolean {
    return (
      obj === null ||
      obj === undefined ||
      (this.isNullUndefEmptyStr(obj.value) &&
        obj.matchMode !== 'empty' &&
        obj.matchMode !== 'notEmpty')
    );
  }

  public cleanFilter(element: FilterMetadata) {
    const elemCopy = { ...(element as FilterMetadata) };
    if (element.matchMode === 'empty' || element.matchMode === 'notEmpty') {
      elemCopy.value = null;
    }
    return elemCopy;
  }

  public clearFilterMetaData(filter: FilterMetadata | FilterMetadata[]) {
    if (Array.isArray(filter)) {
      filter.splice(1);
      this.clearFilterMetaData(filter[0]);
    } else {
      filter.value = null;
      if (filter.matchMode === 'empty' || filter.matchMode === 'notEmpty') {
        filter.matchMode = '';
      }
    }
  }

  public getFillScrollHeightValue(
    layoutService: BiaLayoutService,
    compactMode: boolean,
    showTableController: boolean,
    offset?: string
  ): string {
    let height: string = LayoutHelperService.defaultContainerHeight(
      layoutService,
      offset
    );
    // table header height = 2.24rem

    // Non compact mode :
    // table header margin = 1.25rem
    // controller height = 4.5rem
    // paginator = 3.75rem

    // Compact mode :
    // table header margin = -0.25rem
    // controller height = 3.25rem
    // paginator = 2.6rem

    height += ' - 11.74rem';
    if (compactMode) {
      height += ' + 3.75rem';

      if (!showTableController) {
        height += ' + 3.25rem';
      }
    } else if (!showTableController) {
      height += ' + 4.5rem';
    }

    return `calc(${height})`;
  }
}
