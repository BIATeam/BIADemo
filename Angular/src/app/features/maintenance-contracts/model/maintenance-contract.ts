import { Validators } from '@angular/forms';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { ArchivableDto } from 'src/app/shared/bia-shared/model/dto/archivable-dto';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { FixableDto } from 'src/app/shared/bia-shared/model/dto/fixable-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// TODO after creation of CRUD MaintenanceContract : adapt the model
export interface MaintenanceContract
  extends BaseDto,
    ArchivableDto,
    FixableDto {
  aircraftMaintenanceCompany: OptionDto | null;
  contractNumber: string;
  description: string | null;
  planes: OptionDto[] | null;
  site: OptionDto | null;
}

// TODO after creation of CRUD MaintenanceContract : adapt the field configuration
export const maintenanceContractFieldsConfiguration: BiaFieldsConfig<MaintenanceContract> =
  {
    columns: [
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
      Object.assign(
        new BiaFieldConfig('description', 'maintenanceContract.description'),
        {
          validators: [Validators.maxLength(64)],
        }
      ),
      Object.assign(new BiaFieldConfig('site', 'maintenanceContract.site'), {
        type: PropType.OneToMany,
      }),
      Object.assign(
        new BiaFieldConfig(
          'aircraftMaintenanceCompany',
          'maintenanceContract.aircraftMaintenanceCompany'
        ),
        {
          type: PropType.OneToMany,
        }
      ),
      Object.assign(
        new BiaFieldConfig('planes', 'maintenanceContract.planes'),
        {
          type: PropType.ManyToMany,
        }
      ),
    ],
  };
