import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { BiaTableState } from 'bia-ng/models';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'bia-view-state-recap',
  templateUrl: './view-state-recap.component.html',
  styleUrls: ['./view-state-recap.component.scss'],
  imports: [TranslateModule, TableModule],
})
export class ViewStateRecapComponent implements OnChanges {
  @Input() tableState?: BiaTableState;
  recapData: { column: string; filter: any; sort: string }[] = [];

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
        if (Array.isArray(filter)) {
          return filter.map(f => ({
            field,
            value: f.value ? JSON.stringify(f) : '-',
          }));
        } else {
          return [
            { field, value: filter.value ? JSON.stringify(filter) : '-' },
          ];
        }
      }
    );

    this.recapData =
      columnOrder?.map(col => {
        const filter = filtersArray.find(f => f.field === col);
        const sort = multiSortMeta?.find(s => s.field === col);
        return {
          column: col,
          filter: filter ? filter.value : '-',
          sort: sort ? (sort.order === 1 ? 'Asc' : 'Desc') : '-',
        };
      }) || [];
  }
}
