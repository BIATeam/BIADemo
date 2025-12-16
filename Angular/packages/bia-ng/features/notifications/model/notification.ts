import { BaseDto, OptionDto } from '@bia-team/bia-ng/models';
import { DtoState } from '@bia-team/bia-ng/models/enum';

export interface Notification extends BaseDto {
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

export class NotificationTranslation extends BaseDto {
  languageId: number;
  title: string;
  description: string;

  constructor(
    id: any,
    languageId: number,
    title: string,
    description: string,
    dtoState: DtoState
  ) {
    super(id, dtoState);
    this.languageId = languageId;
    this.title = title;
    this.description = description;
  }
}

export class NotificationTeam extends BaseDto {
  display: string;
  team: OptionDto;
  teamTypeId: number;
  roles: OptionDto[];
}

export interface NotificationData {
  route: string[] | null;
  display: string;
  teams: NotificationTeam[] | null;
}
