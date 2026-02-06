import { BaseDto } from 'packages/bia-ng/models/public-api';

export interface ViewTeam extends BaseDto<number> {
  teamTitle: string;
  isDefault: boolean;
}
