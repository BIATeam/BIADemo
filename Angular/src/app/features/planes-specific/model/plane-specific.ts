import {
  BiaFieldConfig,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  BiaFormLayoutConfigField,
  BiaFormLayoutConfigGroup,
  BiaFormLayoutConfigRow,
  BiaFormLayoutConfigTab,
  BiaFormLayoutConfigTabGroup,
} from 'bia-ng/models';
import { PropType } from 'bia-ng/models/enum';
import { Engine } from './engine';
import { Plane, planeFieldsConfiguration } from './plane';

// TODO after creation of CRUD Plane : adapt the model
export interface PlaneSpecific extends Plane {
  engines: Engine[];
}

// TODO after creation of CRUD Plane : adapt the field configuration
export const planeSpecificFieldsConfiguration: BiaFieldsConfig<PlaneSpecific> =
  {
    columns: [
      ...planeFieldsConfiguration.columns,
      Object.assign(new BiaFieldConfig('engines', 'plane.engines'), {
        specificOutput: true,
        specificInput: true,
        minWidth: '50px',
        type: PropType.ManyToMany,
      }),
    ],
  };

export const planeSpecificFormLayoutConfiguration: BiaFormLayoutConfig<PlaneSpecific> =
  new BiaFormLayoutConfig<PlaneSpecific>(
    [
      new BiaFormLayoutConfigRow([
        new BiaFormLayoutConfigTabGroup([
          new BiaFormLayoutConfigTab('information', 'plane.information', [
            new BiaFormLayoutConfigRow([
              new BiaFormLayoutConfigGroup('plane.groupIdentification', [
                new BiaFormLayoutConfigRow([
                  new BiaFormLayoutConfigField('msn'),
                ]),
              ]),
              new BiaFormLayoutConfigGroup('plane.groupStatus', [
                new BiaFormLayoutConfigRow([
                  new BiaFormLayoutConfigField('isActive', 2),
                ]),
                new BiaFormLayoutConfigRow([
                  new BiaFormLayoutConfigField('connectingAirports'),
                ]),
              ]),
            ]),
            new BiaFormLayoutConfigRow([
              new BiaFormLayoutConfigField('capacity'),
            ]),
          ]),
          new BiaFormLayoutConfigTab('tracking', 'plane.groupTracking', [
            new BiaFormLayoutConfigRow([
              new BiaFormLayoutConfigField('deliveryDate'),
              new BiaFormLayoutConfigField('lastFlightDate'),
            ]),
            new BiaFormLayoutConfigRow([
              new BiaFormLayoutConfigField('syncFlightDataTime'),
            ]),
          ]),
        ]),
        new BiaFormLayoutConfigGroup('plane.engines', [
          new BiaFormLayoutConfigRow([new BiaFormLayoutConfigField('engines')]),
        ]),
      ]),
    ],
    false
  );
