import { DatePipe, NgClass, NgTemplateOutlet } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { BiaFieldConfig } from 'packages/bia-ng/models/bia-field-config';
import { HistoricalEntryDto } from 'packages/bia-ng/models/dto/historical-entry-dto';
import { HistoricalEntryType } from 'packages/bia-ng/models/enum/historical-entry-type.enum';
import { CardModule } from 'primeng/card';
import { TimelineModule } from 'primeng/timeline';

@Component({
  selector: 'bia-crud-item-historical-timeline',
  imports: [
    TimelineModule,
    DatePipe,
    CardModule,
    TranslateModule,
    NgClass,
    NgTemplateOutlet,
  ],
  templateUrl: './crud-item-historical-timeline.component.html',
  styleUrl: './crud-item-historical-timeline.component.scss',
})
export class CrudItemHistoricalTimelineComponent<
  TDto extends { id: number | string },
> {
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
      case HistoricalEntryType.Fixed:
        return 'pi-lock';
      case HistoricalEntryType.Unfixed:
        return 'pi-lock-open';
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
      case HistoricalEntryType.Fixed:
        return 'fix';
      case HistoricalEntryType.Unfixed:
        return 'unfix';
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
      case HistoricalEntryType.Fixed:
        return 'bia.fixed';
      case HistoricalEntryType.Unfixed:
        return 'bia.unfixed';
    }
  }

  getFullPropertyName(propertyName: string) {
    if (propertyName.toLowerCase() === 'isarchived') {
      return 'bia.archived';
    }

    const field = this.fields.find(
      f => f.field.toLowerCase() === propertyName.toLowerCase()
    );
    return field ? field.header : propertyName;
  }

  getDisplayValue(value: any) {
    if (value === 'True') {
      return this.translateService.instant('bia.true');
    }

    if (value === 'False') {
      return this.translateService.instant('bia.false');
    }

    return this.isEmptyValue(value) ? '   ' : value;
  }

  isEmptyValue(value: any): boolean {
    return !value || value === '';
  }

  valueOf(modification: any, kind: 'old' | 'new') {
    return modification?.[`${kind}Value`];
  }

  isEmptyByKind(modification: any, kind: 'old' | 'new') {
    return this.isEmptyValue(this.valueOf(modification, kind));
  }

  iconOf(kind: 'old' | 'new') {
    return kind === 'old' ? 'pi pi-trash' : 'pi pi-plus-circle';
  }
}
