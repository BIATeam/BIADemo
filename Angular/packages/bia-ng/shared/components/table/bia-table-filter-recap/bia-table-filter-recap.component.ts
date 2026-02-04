import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Input,
  ViewEncapsulation,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BiaFieldConfig, OptionDto } from '@bia-team/bia-ng/models';
import { TranslateModule } from '@ngx-translate/core';
import { FilterMetadata } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { FormatValuePipe } from '../../../pipes/format-value.pipe';

@Component({
  selector: 'bia-table-filter-recap',
  templateUrl: './bia-table-filter-recap.component.html',
  styleUrls: ['./bia-table-filter-recap.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  encapsulation: ViewEncapsulation.None,
  imports: [
    TableModule,
    FormsModule,
    TranslateModule,
    FormatValuePipe,
    CommonModule,
  ],
})
export class BiaTableFilterRecapComponent<TDto> {
  @Input() filterArray: FilterMetadata[] | null;
  @Input() columnFilterType: string;
  @Input() field: BiaFieldConfig<TDto>;
  @Input() options?: OptionDto[];

  getOptionsLabels(value: number[]): string {
    return value
      .map(v => this.options?.find(o => o.id === v)?.display)
      .join(',');
  }
}
