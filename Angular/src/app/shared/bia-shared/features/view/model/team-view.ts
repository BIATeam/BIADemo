import { Team } from 'src/app/domains/bia-domains/team/model/team';
import { View } from './view';

export interface TeamView extends View {
  teamId: number;
  team: Team;
}
