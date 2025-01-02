/* eslint-disable @typescript-eslint/naming-convention */
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export interface Notification {
  id: number;
  title: string;
  description: string;
  type: OptionDto;
  read: boolean;
  createdDate: string;
  createdBy: OptionDto | null;
  notifiedUsers: OptionDto[];
  notifiedTeams: NotificationTeam[];
  jData: string;
  data?: NotificationData;
}

export class NotificationTeam {
  display: string;
  team: OptionDto;
  teamTypeId: number;
  roles: OptionDto[];
}

export enum NotificationType {
  Task = 1,
  Info = 2,
  Success = 3,
  Warning = 4,
  Error = 5,
}

export interface NotificationData {
  route: string[] | null;
  display: string;
  teams: NotificationTeam[] | null;
}
