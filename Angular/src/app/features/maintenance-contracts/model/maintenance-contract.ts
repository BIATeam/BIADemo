﻿import { Validators } from '@angular/forms';
import {
  ArchivableDto,
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  FixableDto,
  OptionDto,
  PropType,
} from 'biang/models';

// TODO after creation of CRUD MaintenanceContract : adapt the model
export interface MaintenanceContract
  extends BaseDto,
    ArchivableDto,
    FixableDto {
  aircraftMaintenanceCompanyId: number;
  contractNumber: string;
  description: string | null;
  planes: OptionDto[] | null;
  siteId: number;
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
          maxlength: 64,
          isRequired: true,
          validators: [Validators.maxLength(64)],
        }
      ),
      Object.assign(
        new BiaFieldConfig('description', 'maintenanceContract.description'),
        {
          maxlength: 64,
          validators: [Validators.maxLength(64)],
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
