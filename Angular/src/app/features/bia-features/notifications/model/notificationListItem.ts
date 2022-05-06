import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export interface NotificationListItem {
  id: number;
  titleTranslated: string;
  descriptionTranslated: string;
  type: OptionDto;
  read: boolean;
  createdDate: string;
  createdBy: OptionDto | null;
  siteId: number;
  notifiedUsers: OptionDto[];
  notifiedTeams: OptionDto[];
  jData: string;
}