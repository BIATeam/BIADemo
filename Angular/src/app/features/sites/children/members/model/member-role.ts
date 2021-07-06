import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

export interface MemberRole extends BaseDto {
  memberId: number;
  roleId: number;
}
