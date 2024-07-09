import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { RoleDto } from 'src/app/shared/bia-shared/model/role';

export interface Team extends BaseDto {
  teamTypeId: number;
  title: string;
  isDefault: boolean;
  roles: RoleDto[];
  parentTeamId: number;
  parentTeamTitle: string;
}
