import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
} from 'src/app/shared/bia-shared/model/bia-field-config';

// TODO after creation of CRUD Airport : adapt the model
export interface Airport extends BaseDto {
  name: string;
  city: string;
}

// TODO after creation of CRUD Airport : adapt the field configuration
export const airportFieldsConfiguration: BiaFieldsConfig<Airport> = {
  columns: [
    Object.assign(new BiaFieldConfig('name', 'airport.name'), {
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('city', 'airport.city'), {
      isRequired: true,
    }),
  ],
};
