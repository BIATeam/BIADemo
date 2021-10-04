import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export interface Notification {
  id: number;
  title: string;
  description: string;
  type: OptionDto;
  read: boolean;
  createdDate: string;
  createdBy: OptionDto | null;
  siteId: number;
  notifiedPermissions: OptionDto[];
  notifiedUsers: OptionDto[];
  jData: string;
}

export interface NotificationTarget {
  route: any[];
}
