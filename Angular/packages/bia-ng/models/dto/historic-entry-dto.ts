import { HistoricEntryType } from '../enum/historic-entry-type.enum';

export interface HistoricEntryDto {
  entryType: HistoricEntryType;
  entryDateTime: Date;
  entryUserLogin: string;
  entryModifications: HistoricEntryModification[];
}

export interface HistoricEntryModification {
  propertyName: string;
  oldValue: string;
  newValue: string;
  isLinkedProperty: boolean;
}
