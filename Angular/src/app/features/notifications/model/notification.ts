export interface Notification {
  id: number;
  title: string;
  description: string;
  typeId: number;
  read: boolean;
  createdDate: string;
  createdById: number;
  siteId: number;
  notifiedRoleIds: number[];
  notifiedUserIds: number[];
  targetJson: string;
}
