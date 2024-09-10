import {
  BaseTeamDto,
  teamFieldsConfigurationColumns,
} from 'src/app/shared/bia-shared/model/base-team-dto';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';

// TODO after creation of CRUD Team MaintenanceTeam : adapt the model
export class MaintenanceTeam extends BaseTeamDto {
  /// BIAToolKit - Begin Properties
  code: string;
  isActive: boolean;
  /// BIAToolKit - End Properties
}

// TODO after creation of CRUD Team MaintenanceTeam : adapt the field configuration
export const maintenanceTeamFieldsConfiguration: BiaFieldsConfig = {
  columns: [
    ...teamFieldsConfigurationColumns,
    ...[
      /// BIAToolKit - Begin Block code
      Object.assign(new BiaFieldConfig('code', 'maintenanceTeam.code'), {
        type: PropType.String,
      }),
      /// BIAToolKit - End Block code
      /// BIAToolKit - Begin Block isActive
      Object.assign(
        new BiaFieldConfig('isActive', 'maintenanceTeam.isActive'),
        {
          type: PropType.Boolean,
        }
      ),
      /// BIAToolKit - End Block isActive
    ],
  ],
};
