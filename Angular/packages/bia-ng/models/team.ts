import { BaseDto } from './dto/base-dto';
import { RoleDto } from './role';

export interface Team extends BaseDto {
  teamTypeId: number;
  title: string;
  isDefault: boolean;
  roles: RoleDto[];
  parentTeamId: number;
  parentTeamTitle: string;
}
