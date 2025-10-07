import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
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

  getEntryIcon(entry: HistoricEntryDto): string {
    switch (entry.entryType) {
      case HistoricEntryType.Insert:
        return entry.isLinkedEntity ? 'pi-plus' : 'pi-sparkles';
      case HistoricEntryType.Delete:
        return 'pi-times';
      case HistoricEntryType.Update:
        return 'pi-pencil';
    }
  }

  getEntryIconClassSuffix(entry: HistoricEntryDto): string {
    switch (entry.entryType) {
      case HistoricEntryType.Insert:
        return entry.isLinkedEntity ? 'add-link' : 'creation';
      case HistoricEntryType.Delete:
        return 'delete-link';
      case HistoricEntryType.Update:
        return 'edit';
    }
  }

  getEntryTitle(entry: HistoricEntryDto): string {
    switch (entry.entryType) {
      case HistoricEntryType.Insert:
        return entry.isLinkedEntity ? `Add link` : 'Creation';
      case HistoricEntryType.Delete:
        return `Remove link`;
      case HistoricEntryType.Update:
        return 'Modification';
    }
  }

  getFullPropertyName(propertyName: string) {
    const field = this.fields.find(
      f => f.field.toLowerCase() === propertyName.toLowerCase()
    );
    return field ? field.header : propertyName;
  }
}
