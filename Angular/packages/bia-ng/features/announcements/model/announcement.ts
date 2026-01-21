import { Validators } from '@angular/forms';
import { PropType } from 'packages/bia-ng/models/enum/public-api';
import {
  Announcement,
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  BiaFormLayoutConfigField,
  BiaFormLayoutConfigRow,
} from 'packages/bia-ng/models/public-api';

// TODO after creation of CRUD Announcement : adapt the field configuration
export const announcementFieldsConfiguration: BiaFieldsConfig<Announcement> = {
  columns: [
    Object.assign(new BiaFieldConfig('start', 'announcement.start'), {
      type: PropType.DateTime,
      isRequired: true,
      isSearchable: false,
      displayFormat: Object.assign(new BiaFieldDateFormat(), {
        autoFormatDate: 'dd/MM/yyyy HH:mm:ss',
        autoPrimeDateFormat: 'dd/mm/yy',
        autoHourFormat: '24',
        isLocale: true,
      }),
    }),
    Object.assign(new BiaFieldConfig('end', 'announcement.end'), {
      type: PropType.DateTime,
      isRequired: true,
      isSearchable: false,
      displayFormat: Object.assign(new BiaFieldDateFormat(), {
        autoFormatDate: 'dd/MM/yyyy HH:mm:ss',
        autoPrimeDateFormat: 'dd/mm/yy',
        autoHourFormat: '24',
        isLocale: true,
      }),
    }),
    Object.assign(new BiaFieldConfig('type', 'announcement.type'), {
      type: PropType.OneToMany,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('rawContent', 'announcement.rawContent'), {
      type: PropType.String,
      specificInput: true,
      specificOutput: true,
      isSortable: false,
      validators: [Validators.required, Validators.minLength(1)],
    }),
    Object.assign(new BiaFieldConfig('rowVersion', 'announcement.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};

// TODO after creation of CRUD Announcement : adapt the form layout configuration
export const announcementFormLayoutConfiguration: BiaFormLayoutConfig<Announcement> =
  new BiaFormLayoutConfig(
    [
      new BiaFormLayoutConfigRow([
        new BiaFormLayoutConfigField('start'),
        new BiaFormLayoutConfigField('end'),
        new BiaFormLayoutConfigField('type'),
      ]),
    ],
    false
  );
