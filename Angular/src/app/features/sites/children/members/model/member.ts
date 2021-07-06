import { MemberRole } from './member-role';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

export interface Member extends BaseDto {
  userId: number;
  userFirstName: string;
  userLastName: string;
  userLogin: string;
  siteId: number;
  roles: MemberRole[];
}
