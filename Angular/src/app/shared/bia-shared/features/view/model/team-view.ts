import { View } from './view';
import { Team } from 'src/app/domains/team/model/team';

export interface TeamView extends View {
  teamId: number;
  team: Team;
}
