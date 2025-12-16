import { HistoricalEntryType } from '@bia-team/bia-ng/models/enum';

export interface HistoricalEntryDto {
  entryType: HistoricalEntryType;
  entryDateTime: Date;
  entryUserLogin: string;
  entryModifications: HistoricalEntryModification[];
}

export interface HistoricalEntryModification {
  propertyName: string;
  oldValue: string;
  newValue: string;
  isLinkedProperty: boolean;
}
