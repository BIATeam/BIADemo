import { ViewTeam } from './view-team';

export interface View {
  id: number;
  tableId: string;
  name: string;
  description: string;
  viewType: number;
  isUserDefault: boolean;
  preference: string;
  viewTeams: ViewTeam[];
}
