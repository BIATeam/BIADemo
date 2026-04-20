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
import { FilterMatchMode, FilterOperator } from 'primeng/api';
import { UserTeam } from './user-team';

// TODO after creation of CRUD User : adapt the model
export interface User extends BaseDto {
  lastName: string;
  firstName: string;
  login: string;
  guid: string;
  // Computed by Angular when data receive
  displayName: string;
  roles: OptionDto[];
  teams: UserTeam[];
  isActive: boolean;
}

// TODO after creation of CRUD User : adapt the field configuration
export const userFieldsConfiguration: BiaFieldsConfig<User> = {
  columns: [
    Object.assign(new BiaFieldConfig('lastName', 'user.lastName'), {
      fieldEditMode: FieldEditMode.ReadOnly,
    }),
    Object.assign(new BiaFieldConfig('firstName', 'user.firstName'), {
      fieldEditMode: FieldEditMode.ReadOnly,
    }),
    Object.assign(new BiaFieldConfig('login', 'user.login'), {
      fieldEditMode: FieldEditMode.InitializableOnly,
    }),
    Object.assign(new BiaFieldConfig('teams', 'member.teams'), {
      fieldEditMode: FieldEditMode.ReadOnly,
      specificOutput: true,
      isSortable: false,
      isSearchable: false,
    }),
    Object.assign(new BiaFieldConfig('roles', 'member.roles'), {
      type: PropType.ManyToMany,
    }),
    Object.assign(new BiaFieldConfig('isActive', 'user.isActive'), {
      type: PropType.Boolean,
      fieldEditMode: FieldEditMode.ReadOnly,
      tableColumnVisibility: TableColumnVisibility.AvailableButHidden,
      defaultFilter: [
        {
          value: true,
          matchMode: FilterMatchMode.CONTAINS,
          operator: FilterOperator.AND,
        },
      ],
    }),
  ],
};
