import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  VersionedDto,
} from 'packages/bia-ng/models/public-api';

// TODO after creation of CRUD Airport : adapt the model
export interface Airport extends BaseDto, VersionedDto {
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
    Object.assign(new BiaFieldConfig('rowVersion', 'airport.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};
