import { DatePipe, NgClass, NgTemplateOutlet } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { HistoricalEntryType } from 'packages/bia-ng/models/enum/public-api';
import {
  BiaFieldConfig,
  HistoricalEntryDto,
} from 'packages/bia-ng/models/public-api';
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
  styleUrls: ['./crud-item-historical-timeline.component.scss'],
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
      case HistoricalEntryType.create:
        return 'pi-sparkles';
      case HistoricalEntryType.delete:
        return 'pi-times';
      case HistoricalEntryType.update:
        return 'pi-pencil';
      case HistoricalEntryType.fixed:
        return 'pi-lock';
      case HistoricalEntryType.unfixed:
        return 'pi-lock-open';
    }
  }

  getEntryIconClassSuffix(entry: HistoricalEntryDto): string {
    switch (entry.entryType) {
      case HistoricalEntryType.create:
        return 'add';
      case HistoricalEntryType.delete:
        return 'delete';
      case HistoricalEntryType.update:
        return 'edit';
      case HistoricalEntryType.fixed:
        return 'fix';
      case HistoricalEntryType.unfixed:
        return 'unfix';
    }
  }

  getEntryTitle(entry: HistoricalEntryDto): string {
    switch (entry.entryType) {
      case HistoricalEntryType.create:
        return 'bia.creation';
      case HistoricalEntryType.delete:
        return `bia.deletion`;
      case HistoricalEntryType.update:
        return 'bia.modification';
      case HistoricalEntryType.fixed:
        return 'bia.fixed';
      case HistoricalEntryType.unfixed:
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

  getDisplayValueNotEmpty(value: any) {
    if (value === 'True') {
      return this.translateService.instant('bia.true');
    }

    if (value === 'False') {
      return this.translateService.instant('bia.false');
    }

    return value;
  }

  isEmptyValue(value: any): boolean {
    return !value || value === '';
  }

  modificationValueByKind(modification: any, kind: 'old' | 'new') {
    return modification?.[`${kind}Value`];
  }

  modificationLinkedPropertyIconByKind(kind: 'old' | 'new') {
    return kind === 'old' ? 'pi pi-trash' : 'pi pi-plus-circle';
  }
}
