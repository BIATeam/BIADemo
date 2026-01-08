import { HistoricalEntryType } from 'packages/bia-ng/models/enum/public-api';

export interface HistoricalEntryDto {
  entryType: HistoricalEntryType;
  entryDateTime: Date;
  entryUser: string;
  entryModifications: HistoricalEntryModification[];
}

export interface HistoricalEntryModification {
  propertyName: string;
  oldValue: string;
  newValue: string;
  isLinkedProperty: boolean;
}
