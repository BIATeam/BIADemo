import { BiaListConfig, PrimeNGFiltering, PrimeTableColumn, PropType } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export interface Plane extends BaseDto {
  msn: string;
  isActive: boolean;
  lastFlightDate: Date;
  deliveryDate: Date;
  syncTime: string;
  capacity: number;
  siteId: number;
  connectingAirports: OptionDto[];
  planeType: OptionDto | null;
}

export const PlaneCRUDConfiguration : BiaListConfig = 
{
    columns: [
    Object.assign(new PrimeTableColumn('msn', 'plane.msn'), {
      isRequired: true,
    }),
    Object.assign(new PrimeTableColumn('isActive', 'plane.isActive'), {
      isSearchable: false,
      isSortable: false,
      type: PropType.Boolean,
    }),
    Object.assign(new PrimeTableColumn('lastFlightDate', 'plane.lastFlightDate'), {
      type: PropType.DateTime,
    }),
    Object.assign(new PrimeTableColumn('deliveryDate', 'plane.deliveryDate'), {
      type: PropType.Date,
    }),
    Object.assign(new PrimeTableColumn('syncTime', 'plane.syncTime'), {
      type: PropType.TimeSecOnly,
    }),
    Object.assign(new PrimeTableColumn('capacity', 'plane.capacity'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      isRequired: true,
    }),
    Object.assign(new PrimeTableColumn('planeType', 'plane.planeType'), {
      type: PropType.OneToMany,
    }),
    Object.assign(new PrimeTableColumn('connectingAirports', 'plane.connectingAirports'), {
      type: PropType.ManyToMany,
    })
  ]
}