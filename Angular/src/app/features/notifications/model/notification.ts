import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { NotificationTranslation } from './notification-translation';

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
  notifiedPermissions: OptionDto[];
  notifiedUsers: OptionDto[];
  notifiedTeams: OptionDto[];
  jData: string;
  data: NotificationData;
  notificationTranslations: NotificationTranslation[];
}

export class NotificationTeam {
  id: number;
  typeId: number;
  display: string;
}

export interface NotificationData {
  route: string[];
  display: string;
  teams: NotificationTeam[]
}