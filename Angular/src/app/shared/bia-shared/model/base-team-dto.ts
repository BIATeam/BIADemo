import { BaseDto } from './base-dto';

export class BaseTeamDto extends BaseDto {
  title: string;
  canUpdate: boolean;
  canMemberListAccess: boolean;
}
