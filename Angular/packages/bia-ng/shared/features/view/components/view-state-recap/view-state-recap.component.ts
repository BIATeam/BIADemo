import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import {
  BiaFieldConfig,
  BiaTableState,
} from 'packages/bia-ng/models/public-api';
import { FilterMetadata } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { BiaTableFilterRecapComponent } from '../../../../components/table/bia-table-filter-recap/bia-table-filter-recap.component';
import { DictOptionDto } from '../../../../components/table/bia-table/dict-option-dto';

@Component({
  selector: 'bia-view-state-recap',
  templateUrl: './view-state-recap.component.html',
  styleUrls: ['./view-state-recap.component.scss'],
  imports: [
    TranslateModule,
    TableModule,
    CommonModule,
    BiaTableFilterRecapComponent,
  ],
})
export class ViewStateRecapComponent<TDto> implements OnChanges {
  @Input() tableState?: BiaTableState;
  @Input() fieldsConfig?: BiaFieldConfig<TDto>[];
  @Input() dictOptionDtos?: DictOptionDto[];
  recapData: { column: string; filter: any; sort: string }[] = [];
  arrayType: typeof Array = Array;
  globalFilter?: any;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['tableState']) {
      this.updateRecapData();
    }
  }

  private updateRecapData() {
    if (!this.tableState) {
      this.recapData = [];
      return;
    }

    const { filters, columnOrder, multiSortMeta } = this.tableState;
    const filtersArray = Object.entries(filters || {}).flatMap(
      ([field, filter]) => {
        if (!filter) return [];
        return [{ field, value: filter ? filter : undefined }];
      }
    );

    this.globalFilter = (
      filtersArray.find(f => f.field.startsWith('global|'))
        ?.value as FilterMetadata
    )?.value;

    this.recapData =
      columnOrder?.map(col => {
        const filter = filtersArray.find(f => f.field === col);
        const sort = multiSortMeta?.find(s => s.field === col);
        return {
          column: col,
          filter: filter ? filter.value : '',
          sort: sort ? (sort.order === 1 ? 'pi-sort-up' : 'pi-sort-down') : '',
        };
      }) || [];
  }

  public getOptionDto(key: string) {
    return this.dictOptionDtos?.filter(x => x.key === key)[0]?.value;
  }

  public getFieldConfig(key: string) {
    return this.fieldsConfig?.filter(x => x.field === key)[0];
  }
}
