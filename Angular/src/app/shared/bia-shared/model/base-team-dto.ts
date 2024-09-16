import { BaseDto } from './base-dto';
import { BiaFieldConfig } from './bia-field-config';

export class BaseTeamDto extends BaseDto {
  title: string;
  canUpdate: boolean;
  canMemberListAccess: boolean;
}

export const teamFieldsConfigurationColumns: BiaFieldConfig[] = [
  Object.assign(new BiaFieldConfig('title', 'site.title'), {
    isRequired: true,
  }),
];
