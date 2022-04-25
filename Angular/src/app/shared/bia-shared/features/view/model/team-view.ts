import { View } from './view';
import { Team } from 'src/app/domains/bia-domains/team/model/team';

export interface TeamView extends View {
  teamId: number;
  team: Team;
}
