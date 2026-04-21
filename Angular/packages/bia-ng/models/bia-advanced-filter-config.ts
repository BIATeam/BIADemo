import { Observable } from 'rxjs';
import { BiaFieldDateFormat, BiaFieldNumberFormat } from './bia-field-config';
import { PropType } from './enum/public-api';
import { OptionDto } from './option-dto';

export class BiaAdvancedFilterFieldConfig<TAdvancedFilter> {
  field: keyof TAdvancedFilter & string;
  header: string;
  type: PropType;
  options?: OptionDto[];
  options$?: Observable<OptionDto[]>;
  numberFormat?: BiaFieldNumberFormat;
  dateFormat?: BiaFieldDateFormat;
  allowSelectFilter?: boolean;
  specificInput?: boolean;

  constructor(
    field: keyof TAdvancedFilter & string,
    header: string,
    type: PropType
  ) {
    this.field = field;
    this.header = header;
    this.type = type;
  }
}

export interface BiaAdvancedFilterConfig<TAdvancedFilter> {
  fields: BiaAdvancedFilterFieldConfig<TAdvancedFilter>[];
}
