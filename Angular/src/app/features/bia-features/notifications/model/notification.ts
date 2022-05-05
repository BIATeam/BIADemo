import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export interface Notification {
  id: number;
  title: string;
  titleTranslated: string;
  description: string;
  descriptionTranslated: string;
  type: OptionDto;
  read: boolean;
  createdDate: string;
  createdBy: OptionDto | null;
  siteId: number;
  notifiedUsers: OptionDto[];
  notifiedTeams: NotificationTeam[];
  jData: string;
  data: NotificationData;
  notificationTranslations: NotificationTranslation[];
}

export interface NotificationTranslation extends BaseDto {
  languageId: number;
  title: string;
  description: string;
}

export class NotificationTeam extends BaseDto {
  display: string;
  team: OptionDto;
  teamTypeId: number;
  roles: OptionDto[];
}

export interface NotificationData {
  route: string[];
  display: string;
  teams: NotificationTeam[]
}