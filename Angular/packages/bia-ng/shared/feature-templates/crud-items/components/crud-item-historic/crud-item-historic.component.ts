import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { BiaFieldConfig } from 'packages/bia-ng/models/bia-field-config';
import { HistoricEntryDto } from 'packages/bia-ng/models/dto/historic-entry-dto';
import { HistoricEntryType } from 'packages/bia-ng/models/enum/historic-entry-type.enum';
import { CardModule } from 'primeng/card';
import { TimelineModule } from 'primeng/timeline';

@Component({
  selector: 'bia-crud-item-historic',
  imports: [TimelineModule, DatePipe, CardModule, TranslateModule],
  templateUrl: './crud-item-historic.component.html',
  styleUrl: './crud-item-historic.component.scss',
})
export class CrudItemHistoricComponent<TDto extends { id: number | string }> {
  @Input() historicEntries: HistoricEntryDto[] = [];
  @Input() fields: BiaFieldConfig<TDto>[];

  constructor(protected translateService: TranslateService) {}

  getEntryIcon(entry: HistoricEntryDto): string {
    switch (entry.entryType) {
      case HistoricEntryType.Insert:
        return 'pi-sparkles';
      case HistoricEntryType.Delete:
        return 'pi-times';
      case HistoricEntryType.Update:
        return 'pi-pencil';
    }
  }

  getEntryIconClassSuffix(entry: HistoricEntryDto): string {
    switch (entry.entryType) {
      case HistoricEntryType.Insert:
        return 'add';
      case HistoricEntryType.Delete:
        return 'delete';
      case HistoricEntryType.Update:
        return 'edit';
    }
  }

  getEntryTitle(entry: HistoricEntryDto): string {
    switch (entry.entryType) {
      case HistoricEntryType.Insert:
        return 'bia.creation';
      case HistoricEntryType.Delete:
        return `bia.deletion`;
      case HistoricEntryType.Update:
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
