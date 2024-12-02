import { BaseDto } from './base-dto';
import { BiaFieldConfig } from './bia-field-config';
import { OptionDto } from './option-dto';

export class BaseTeamDto extends BaseDto {
  title: string;
  canUpdate: boolean;
  canMemberListAccess: boolean;
  admins: OptionDto[];
}

export const teamFieldsConfigurationColumns: BiaFieldConfig<BaseTeamDto>[] = [
  Object.assign(new BiaFieldConfig<BaseTeamDto>('title', 'site.title'), {
    isRequired: true,
  }),
];
