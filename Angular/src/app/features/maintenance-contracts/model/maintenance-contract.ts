import { Validators } from '@angular/forms';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// TODO after creation of CRUD MaintenanceContract : adapt the model
export class MaintenanceContract extends BaseDto {
  // BIAToolKit - Begin Properties
  aircraftMaintenanceCompany: OptionDto | null;
  archivedDate: Date | null;
  contractNumber: string;
  description: string | null;
  fixedDate: Date | null;
  isArchived: boolean | null;
  planes: OptionDto[] | null;
  site: OptionDto | null;
  // BIAToolKit - End Properties
}

// TODO after creation of CRUD MaintenanceContract : adapt the field configuration
export const maintenanceContractFieldsConfiguration: BiaFieldsConfig<MaintenanceContract> =
  {
    columns: [
      // BIAToolKit - Begin Block contractNumber
      Object.assign(
        new BiaFieldConfig(
          'contractNumber',
          'maintenanceContract.contractNumber'
        ),
        {
          isRequired: true,
          validators: [Validators.required, Validators.maxLength(64)],
        }
      ),
      // BIAToolKit - End Block contractNumber
      // BIAToolKit - Begin Block description
      Object.assign(
        new BiaFieldConfig('description', 'maintenanceContract.description'),
        {
          validators: [Validators.maxLength(64)],
        }
      ),
      // BIAToolKit - End Block description
      // BIAToolKit - Begin Block site
      Object.assign(new BiaFieldConfig('site', 'maintenanceContract.site'), {
        type: PropType.OneToMany,
      }),
      // BIAToolKit - End Block site
      // BIAToolKit - Begin Block aircraftMaintenanceCompany
      Object.assign(
        new BiaFieldConfig(
          'aircraftMaintenanceCompany',
          'maintenanceContract.aircraftMaintenanceCompany'
        ),
        {
          type: PropType.OneToMany,
        }
      ),
      // BIAToolKit - End Block aircraftMaintenanceCompany
      // BIAToolKit - Begin Block maintenanceContracts
      Object.assign(
        new BiaFieldConfig('planes', 'maintenanceContract.planes'),
        {
          type: PropType.ManyToMany,
        }
      ),
      // BIAToolKit - End Block maintenanceContracts
    ],
  };
