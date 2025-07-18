import { RoleMode } from '../role-mode.enum';

export interface TeamEnvironment {
  teamTypeId: number;
  label: string;
  roleMode: RoleMode;
  inHeader: boolean;
  displayAlways: boolean;
  displayLabel: boolean;
  teamSelectionCanBeEmpty: boolean;
}
