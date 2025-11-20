import { Validators } from '@angular/forms';
import { PropType } from 'packages/bia-ng/models/enum/public-api';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  BiaFormLayoutConfigField,
  BiaFormLayoutConfigRow,
  OptionDto,
  VersionedDto,
} from 'packages/bia-ng/models/public-api';

// TODO after creation of CRUD Annoucement : adapt the model
export interface Annoucement extends BaseDto, VersionedDto {
  end: Date;
  rawContent: string;
  start: Date;
  type: OptionDto;
}

// TODO after creation of CRUD Annoucement : adapt the field configuration
export const annoucementFieldsConfiguration: BiaFieldsConfig<Annoucement> = {
  columns: [
    Object.assign(new BiaFieldConfig('start', 'annoucement.start'), {
      type: PropType.DateTime,
      isRequired: true,
      isSearchable: false,
    }),
    Object.assign(new BiaFieldConfig('end', 'annoucement.end'), {
      type: PropType.DateTime,
      isRequired: true,
      isSearchable: false,
    }),
    Object.assign(new BiaFieldConfig('rawContent', 'annoucement.rawContent'), {
      type: PropType.String,
      specificInput: true,
      specificOutput: true,
      isSortable: false,
      validators: [Validators.required, Validators.minLength(1)],
    }),
    Object.assign(new BiaFieldConfig('type', 'annoucement.type'), {
      type: PropType.OneToMany,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('rowVersion', 'annoucement.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};

// TODO after creation of CRUD Annoucement : adapt the form layout configuration
export const annoucementFormLayoutConfiguration: BiaFormLayoutConfig<Annoucement> =
  new BiaFormLayoutConfig([
    new BiaFormLayoutConfigRow([
      new BiaFormLayoutConfigField('start'),
      new BiaFormLayoutConfigField('end'),
      new BiaFormLayoutConfigField('type'),
    ]),
  ]);
