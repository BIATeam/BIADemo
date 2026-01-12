import { PropType } from 'packages/bia-ng/models/enum/public-api';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  FixableDto,
  OptionDto,
  VersionedDto,
} from 'packages/bia-ng/models/public-api';

// TODO after creation of CRUD Flight : adapt the model
export interface Flight extends BaseDto<string>, VersionedDto, FixableDto {
  siteId: number;
  departureAirport: OptionDto;
  arrivalAirport: OptionDto;
}

// TODO after creation of CRUD Flight : adapt the field configuration
export const flightFieldsConfiguration: BiaFieldsConfig<Flight> = {
  columns: [
    Object.assign(new BiaFieldConfig('id', 'flight.id'), {
      type: PropType.String,
      isOnlyInitializable: true,
      // Begin BIAToolKit Generation Ignore
      isRequired: true,
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(
      new BiaFieldConfig('departureAirport', 'flight.departureAirport'),
      {
        type: PropType.OneToMany,
        isRequired: true,
      }
    ),
    Object.assign(
      new BiaFieldConfig('arrivalAirport', 'flight.arrivalAirport'),
      {
        type: PropType.OneToMany,
        isRequired: true,
      }
    ),
    Object.assign(new BiaFieldConfig('rowVersion', 'flight.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};

// TODO after creation of CRUD Flight : adapt the form layout configuration
export const flightFormLayoutConfiguration: BiaFormLayoutConfig<Flight> =
  new BiaFormLayoutConfig([]);
