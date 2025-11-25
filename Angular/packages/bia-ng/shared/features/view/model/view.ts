import { PropType } from 'packages/bia-ng/models/enum/public-api';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldMultilineString,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  BiaFormLayoutConfigField,
  BiaFormLayoutConfigRow,
} from 'packages/bia-ng/models/public-api';
import { ViewValidator } from '../validators/view.validator';
import { ViewTeam } from './view-team';

export interface View extends BaseDto {
  tableId: string;
  name: string;
  description: string;
  viewType: number;
  isUserDefault: boolean;
  preference: string;
  viewTeams: ViewTeam[];
}

export const viewFieldsConfiguration: BiaFieldsConfig<View> = {
  columns: [
    Object.assign(new BiaFieldConfig('name', 'bia.views.name'), {
      type: PropType.String,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('description', 'bia.views.description'), {
      type: PropType.String,
      multiline: Object.assign(new BiaFieldMultilineString(), {
        rows: 3,
      }),
    }),
    Object.assign(new BiaFieldConfig('viewType', 'bia.views.viewType'), {
      type: PropType.Number,
      specificInput: true,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('viewTeams', 'bia.views.viewType'), {
      type: PropType.ManyToMany,
      specificInput: true,
    }),
  ],
  formValidators: [ViewValidator.teamViewWithTeams()],
};

export const viewFormLayoutConfiguration: BiaFormLayoutConfig<View> =
  new BiaFormLayoutConfig([
    new BiaFormLayoutConfigRow([new BiaFormLayoutConfigField('name')]),
    new BiaFormLayoutConfigRow([new BiaFormLayoutConfigField('description')]),
    new BiaFormLayoutConfigRow([new BiaFormLayoutConfigField('viewType')]),
  ]);
