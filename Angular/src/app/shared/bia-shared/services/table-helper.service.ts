import { Injectable } from '@angular/core';
import { FilterMetadata } from 'primeng/api';
import { TableLazyLoadEvent } from 'primeng/table';
import { TABLE_FILTER_GLOBAL } from '../../constants';
import { BiaLayoutService } from '../components/layout/services/layout.service';
import { BiaTableComponent } from '../components/table/bia-table/bia-table.component';
import { clone } from '../utils';

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
        obj.matchMode != 'empty' &&
        obj.matchMode != 'notEmpty')
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
    let height: string;
    if (layoutService._config.classicStyle) {
      height = layoutService.state.fullscreen
        ? '100vh - 240px'
        : '100vh - 460px';
    } else {
      if (layoutService.state.fullscreen) {
        height = '100vh - 72px - 11.5rem';
      } else {
        height = '100vh - 69px - 18.167rem';
        height +=
          layoutService._config.footerMode != 'overlay'
            ? ' - var(--footer-height)'
            : '';
      }
      if (layoutService._config.menuMode === 'horizontal') {
        height += ' - 3rem';
      }
    }
    if (compactMode) {
      height += ' - 0.208rem + 58px';

      if (!showTableController) {
        height += ' + 2.545rem + 10px';
      }
    } else if (!showTableController) {
      height += ' + 2.285rem + 35px';
    }

    if (offset) {
      height += ` ${offset}`;
    }
    return `calc(${height})`;
  }
}
