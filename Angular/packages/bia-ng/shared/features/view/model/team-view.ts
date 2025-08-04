import { Team } from 'packages/bia-ng/models/public-api';
import { View } from './view';

export interface TeamView extends View {
  teamId: number;
  team: Team;
}
