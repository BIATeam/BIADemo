import { Component, HostBinding, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaListConfig,
  PrimeTableColumn,
  PropType
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { ActivatedRoute, Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import { PagingFilterFormatDto } from 'src/app/shared/bia-shared/model/paging-filter-format';
import { CrudItemTableComponent } from '../../components/crud-item-table/crud-item-table.component';
import { useCalcMode, useSignalR, useView, useViewTeamWithTypeId } from '../../crud-item.constants';
import { skip } from 'rxjs/operators';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudItemFacadeService } from '../../services/crud-item-facade.service';

@Component({
  selector: 'app-crud-items-index',
  templateUrl: './crud-items-index.component.html',
  styleUrls: ['./crud-items-index.component.scss']
})
export class CrudItemsIndexComponent<CrudItem extends BaseDto> implements OnInit, OnDestroy {
  useCalcMode = useCalcMode;
  useSignalR = useSignalR;
  useView = useView;
  useRefreshAtLanguageChange = false;

  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) biaTableComponent: BiaTableComponent;
  @ViewChild(CrudItemTableComponent, { static: false }) crudItemTableComponent: CrudItemTableComponent<CrudItem>;
  protected get crudItemListComponent() {
    if (this.biaTableComponent !== undefined) {
      return this.biaTableComponent;
    }
    return this.crudItemTableComponent;
  }

  protected sub = new Subscription();
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  crudItems$: Observable<CrudItem[]>;
  selectedCrudItems: CrudItem[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canEdit = false;
  canDelete = false;
  canAdd = false;
  tableConfiguration: BiaListConfig = { columns : []};
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  popupTitle: string;
  tableStateKey = this.useView ? 'crud-itemsGrid' : undefined;
  tableState: string;
  sortFieldValue = 'msn';
  useViewTeamWithTypeId = this.useView ? useViewTeamWithTypeId : null;
  parentIds: string[];

  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    public activatedRoute: ActivatedRoute,
    protected translateService: TranslateService,
    protected biaTranslationService: BiaTranslationService,
    protected facadeService: CrudItemFacadeService<CrudItem>, 
  ) {
  }

  ngOnInit() {
    this.parentIds = [];
    this.sub = new Subscription();

    this.initTableConfiguration();
    this.setPermissions();
    this.crudItems$ = this.facadeService.crudItems$;
    this.totalCount$ = this.facadeService.totalCount$;
    this.loading$ = this.facadeService.loadingGetAll$;
    this.OnDisplay();
    if (this.useCalcMode) {
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(event => {
          this.facadeService.optionsService.loadAllOptions();
        })
      );
    }
    if (this.useRefreshAtLanguageChange) {
      // Reload data if language change.
      this.sub.add(
        this.biaTranslationService.currentCulture$.pipe(skip(1)).subscribe(event => {
          this.onLoadLazy(this.crudItemListComponent.getLazyLoadMetadata());
        })
      );
    }
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
    this.OnHide();
  }

  OnDisplay() {
    if (this.useView) {
      this.store.dispatch(loadAllView());
    }


    if (this.useSignalR) {
      this.facadeService.signalRService.initialize(this.facadeService);
    }
  }

  OnHide() {
    if (this.useSignalR) {
      this.facadeService.signalRService.destroy();
    }
  }

  onCreate() {
    if (!this.useCalcMode) {
      this.router.navigate(['create'], { relativeTo: this.activatedRoute });
    }
  }

  onEdit(crudItemId: number) {
    if (!this.useCalcMode) {
      this.router.navigate([crudItemId, 'edit'], { relativeTo: this.activatedRoute });
    }
  }

  onSave(crudItem: CrudItem) {
    if (this.useCalcMode) {
      if (crudItem.id > 0) {
        if (this.canEdit) {
          this.facadeService.update(crudItem);
        }
      } else {
        if (this.canAdd) {
          this.facadeService.create(crudItem);
        }
      }
    }
  }

  onDelete() {
    if (this.selectedCrudItems && this.canDelete) {
      this.facadeService.multiRemove(this.selectedCrudItems.map((x) => x.id));
    }
  }

  onSelectedElementsChanged(crudItems: CrudItem[]) {
    this.selectedCrudItems = crudItems;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    const pagingAndFilter: PagingFilterFormatDto = { parentIds: this.parentIds, ...lazyLoadEvent };
    this.facadeService.loadAllByPost(pagingAndFilter);
  }

  searchGlobalChanged(value: string) {
    this.globalSearchValue = value;
  }

  displayedColumnsChanged(values: KeyValuePair[]) {
    this.displayedColumns = values;
  }

  onToggleSearch() {
    this.showColSearch = !this.showColSearch;
  }

  onViewChange(viewPreference: string) {
    this.viewPreference = viewPreference;
  }

  onStateSave(tableState: string) {
    this.viewPreference = tableState;
    this.tableState = tableState;
  }

  onExportCSV() {
    const columns: { [key: string]: string } = {};
    this.crudItemListComponent.getPrimeNgTable().columns.map((x: PrimeTableColumn) => (columns[x.field] = this.translateService.instant(x.header)));
    const columnsAndFilter: PagingFilterFormatDto = {
      parentIds: this.parentIds, columns: columns, ...this.crudItemListComponent.getLazyLoadMetadata()
    };
    this.facadeService.dasService.getFile(columnsAndFilter).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.crudItems') + '.csv');
    });
  }

  protected setPermissions() {
    // TODO redefine in plane
    this.canEdit = true;
    this.canDelete = true;
    this.canAdd = true;
  }
  protected initTableConfiguration() {
    this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: this.tableConfiguration.columns.map<PrimeTableColumn>(object => object.clone())}
 
      this.tableConfiguration.columns.forEach(column => {
        switch (column.type)
        {
          case PropType.DateTime :
            column.formatDate = dateFormat.dateTimeFormat;
            break;
          case PropType.Date :
            column.formatDate = dateFormat.dateFormat;
            break;
          case PropType.Time :
            column.formatDate = dateFormat.timeFormat;
            break;
          case PropType.TimeOnly :
            column.formatDate = dateFormat.timeFormat;
            break;
          case PropType.TimeSecOnly :
            column.formatDate = dateFormat.timeFormatSec;
            break;
        }
      });
      
      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    }));
  }
}
