import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { BiaFieldConfig } from 'packages/bia-ng/models/bia-field-config';
import { HistoricalEntryDto } from 'packages/bia-ng/models/dto/historical-entry-dto';
import { HistoricalEntryType } from 'packages/bia-ng/models/enum/historical-entry-type.enum';
import { CardModule } from 'primeng/card';
import { TimelineModule } from 'primeng/timeline';

@Component({
  selector: 'bia-crud-item-historical',
  imports: [TimelineModule, DatePipe, CardModule, TranslateModule],
  templateUrl: './crud-item-historical.component.html',
  styleUrl: './crud-item-historical.component.scss',
})
export class CrudItemHistoricalComponent<TDto extends { id: number | string }> {
  @Input() historicalEntries: HistoricalEntryDto[] = [];
  @Input() fields: BiaFieldConfig<TDto>[];
  historicalEntryType = HistoricalEntryType;

  constructor(protected translateService: TranslateService) {}

  getEntryIcon(entry: HistoricalEntryDto): string {
    switch (entry.entryType) {
      case HistoricalEntryType.Create:
        return 'pi-sparkles';
      case HistoricalEntryType.Delete:
        return 'pi-times';
      case HistoricalEntryType.Update:
        return 'pi-pencil';
    }
  }

  getEntryIconClassSuffix(entry: HistoricalEntryDto): string {
    switch (entry.entryType) {
      case HistoricalEntryType.Create:
        return 'add';
      case HistoricalEntryType.Delete:
        return 'delete';
      case HistoricalEntryType.Update:
        return 'edit';
    }
  }

  getEntryTitle(entry: HistoricalEntryDto): string {
    switch (entry.entryType) {
      case HistoricalEntryType.Create:
        return 'bia.creation';
      case HistoricalEntryType.Delete:
        return `bia.deletion`;
      case HistoricalEntryType.Update:
        return 'bia.modification';
    }
  }

  getFullPropertyName(propertyName: string) {
    const field = this.fields.find(
      f => f.field.toLowerCase() === propertyName.toLowerCase()
    );
    return field ? field.header : propertyName;
  }
}
