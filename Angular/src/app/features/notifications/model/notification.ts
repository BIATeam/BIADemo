import { Role } from 'src/app/domains/role/model/role';
import { Site } from 'src/app/domains/site/model/site';
import { User } from 'src/app/domains/user/model/user';
import { NotificationType } from './notification-type';

export interface Notification {
  id: number;
  jobId: string;
  title: string;
  description: string;
  typeId: number;
  type: NotificationType | string;
  read: boolean;
  createdDate: string;
  createdById: number;
  createdBy: User;
  notifiedRoleId: number | null;
  notifiedRole: Role;
  siteId: number;
  site: Site;
  notificationUsers: User[];
  targetRoute: string;
  targetId: number;
}
