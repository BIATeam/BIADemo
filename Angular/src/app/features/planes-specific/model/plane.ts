import { PrimeNGFiltering, PrimeTableColumn, PropType } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { CrudConfig } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { featureName, useCalcMode, useOfflineMode, usePopup, useSignalR, useView, useViewTeamWithTypeId } from '../plane.constants';

// TODO after CRUD creation : adapt the model
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

// TODO after CRUD creation : adapt the configuration
export const PlaneCRUDConfiguration : CrudConfig = new CrudConfig(
  {
    featureName: featureName,
    columns: [
      Object.assign(new PrimeTableColumn('msn', 'plane.msn'), {
        isRequired: true,
      }),
      Object.assign(new PrimeTableColumn('isActive', 'plane.isActive'), {
        isSearchable: false,
        isSortable: false,
        specificOutput: true,
        specificInput:true,
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
        specificOutput: true,
        specificInput:true,
        isRequired: true,
      }),
      Object.assign(new PrimeTableColumn('planeType', 'plane.planeType'), {
        type: PropType.OneToMany,
      }),
      Object.assign(new PrimeTableColumn('connectingAirports', 'plane.connectingAirports'), {
        type: PropType.ManyToMany,
      })
    ],
    useCalcMode: useCalcMode,
    useSignalR: useSignalR,
    useView: useView,
    useViewTeamWithTypeId: useViewTeamWithTypeId,
    usePopup: usePopup,
    useOfflineMode: useOfflineMode,
    // IMPORTANT: this key should be unique in all the application (except if share same store with same object format).
    // storeKey: 'feature-' + featureName,
    // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
    // tableStateKey: featureName + 'Grid',
  }
)