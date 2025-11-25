import { BaseDto } from 'bia-ng/models';
import { ViewTeam } from './view-team';

export interface View extends BaseDto {
  tableId: string;
  name: string;
  description: string;
  viewType: number;
  isUserDefault: boolean;
  preference: string;
  viewTeams: ViewTeam[];
}
