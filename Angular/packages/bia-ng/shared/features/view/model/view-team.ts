import { BaseDto } from '@bia-team/bia-ng/models';

export interface ViewTeam extends BaseDto<number> {
  teamTitle: string;
  isDefault: boolean;
}
