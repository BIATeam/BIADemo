import { PropType } from 'packages/bia-ng/models/enum/public-api';
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
  Object.assign(new BiaFieldConfig('admins', 'bia.team.admins'), {
    type: PropType.ManyToMany,
    isEditable: false,
    isVisible: false,
  }),
];
