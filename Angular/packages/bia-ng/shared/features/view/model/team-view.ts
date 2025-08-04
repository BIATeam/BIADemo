import { Team } from 'bia-ng/models';
import { View } from './view';

export interface TeamView extends View {
  teamId: number;
  team: Team;
}
