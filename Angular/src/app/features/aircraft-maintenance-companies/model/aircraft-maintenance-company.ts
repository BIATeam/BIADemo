import { BiaFieldConfig, BiaFieldsConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the model
export interface AircraftMaintenanceCompany extends BaseDto {
  title: string;
}

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the field configuration
export const AircraftMaintenanceCompanyFieldsConfiguration : BiaFieldsConfig =
{
  columns: [
    Object.assign(new BiaFieldConfig('title', 'site.title'), {
      isRequired: true,
    }),
  ]
}