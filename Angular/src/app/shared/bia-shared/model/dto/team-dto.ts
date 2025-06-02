import { BiaFieldConfig } from '../bia-field-config';
import { OptionDto } from '../option-dto';

export interface TeamDto {
  title: string;
  canUpdate: boolean;
  canMemberListAccess: boolean;
  admins: OptionDto[];
}

export const teamFieldsConfigurationColumns: BiaFieldConfig<TeamDto>[] = [
  Object.assign(new BiaFieldConfig<TeamDto>('title', 'site.title'), {
    isRequired: true,
  }),
];
