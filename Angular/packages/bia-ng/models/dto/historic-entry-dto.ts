import { HistoricEntryType } from '../enum/historic-entry-type.enum';

export interface HistoricEntryDto {
  entryType: HistoricEntryType;
  entryDateTime: Date;
  entryUserLogin: string;
  modifications: HistoricEntryModification[];
  isLinkedEntity: boolean;
  linkedEntityDisplayValue: string | undefined;
  linkedEntityPropertyName: string | undefined;
}

export interface HistoricEntryModification {
  propertyName: string;
  oldValue: string;
  newValue: string;
}
