import { DateFormat } from 'biang/core';
import { BiaFieldConfig, BiaFieldDateFormat } from 'biang/models';
import { PropType } from 'biang/models/enum';

export class BiaFieldHelperService {
  public static setDateFormat<T>(
    field: BiaFieldConfig<T>,
    dateFormat: DateFormat
  ) {
    if (
      !(field.displayFormat instanceof BiaFieldDateFormat) ||
      !field.customDisplayFormat
    ) {
      field.customDisplayFormat = false;
      field.displayFormat = new BiaFieldDateFormat();
      switch (field.type) {
        case PropType.DateTime:
          field.displayFormat.autoPrimeDateFormat = dateFormat.primeDateFormat;
          field.displayFormat.autoHourFormat = dateFormat.hourFormat;
          field.displayFormat.autoFormatDate = dateFormat.dateTimeFormat;
          break;
        case PropType.Date:
          field.displayFormat.autoPrimeDateFormat = dateFormat.primeDateFormat;
          field.displayFormat.autoFormatDate = dateFormat.dateFormat;
          break;
        case PropType.Time:
          field.displayFormat.autoPrimeDateFormat = dateFormat.timeFormat;
          field.displayFormat.autoHourFormat = dateFormat.hourFormat;
          field.displayFormat.autoFormatDate = dateFormat.timeFormat;
          break;
        case PropType.TimeOnly:
          field.displayFormat.autoPrimeDateFormat = dateFormat.timeFormat;
          field.displayFormat.autoHourFormat = dateFormat.hourFormat;
          field.displayFormat.autoFormatDate = dateFormat.timeFormat;
          break;
        case PropType.TimeSecOnly:
          field.displayFormat.autoPrimeDateFormat = dateFormat.timeFormatSec;
          field.displayFormat.autoHourFormat = dateFormat.hourFormat;
          field.displayFormat.autoFormatDate = dateFormat.timeFormatSec;
          break;
      }
    }
  }
}
