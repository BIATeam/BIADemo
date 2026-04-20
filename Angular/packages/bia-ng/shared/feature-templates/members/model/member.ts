import {
  FieldEditMode,
  PropType,
  TableColumnVisibility,
} from 'packages/bia-ng/models/enum/public-api';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  OptionDto,
} from 'packages/bia-ng/models/public-api';
import { FieldValidator } from '../../../validators/field.validator';

// TODO after creation of CRUD Member : adapt the model
export interface Member extends BaseDto {
  user: OptionDto;
  lastName: string;
  firstName: string;
  login: string;
  isActive: boolean;
  roles: OptionDto[];
  teamId: number;
}

export class Members {
  users: OptionDto[];
  roles: OptionDto[];
  teamId: number;
}

// TODO after creation of CRUD Member : adapt the field configuration
export const memberFieldsConfiguration: BiaFieldsConfig<Member> = {
  formValidators: [FieldValidator.atLeastOneFilled(['user', 'login'])],
  columns: [
    Object.assign(new BiaFieldConfig<Member>('user', 'member.user'), {
      //isRequired: true,
      type: PropType.OneToMany,
    }),
    Object.assign(new BiaFieldConfig('lastName', 'user.lastName'), {
      fieldEditMode: FieldEditMode.ReadOnly,
      tableColumnVisibility: TableColumnVisibility.AvailableButHidden,
    }),
    Object.assign(new BiaFieldConfig('firstName', 'user.firstName'), {
      fieldEditMode: FieldEditMode.ReadOnly,
      tableColumnVisibility: TableColumnVisibility.AvailableButHidden,
    }),
    Object.assign(new BiaFieldConfig('login', 'user.login'), {
      fieldEditMode: FieldEditMode.InitializableOnly,
    }),
    Object.assign(new BiaFieldConfig('isActive', 'member.isActive'), {
      fieldEditMode: FieldEditMode.ReadOnly,
      type: PropType.Boolean,
      tableColumnVisibility: TableColumnVisibility.AvailableButHidden,
    }),
    Object.assign(new BiaFieldConfig<Member>('roles', 'member.roles'), {
      type: PropType.ManyToMany,
    }),
  ],
};
