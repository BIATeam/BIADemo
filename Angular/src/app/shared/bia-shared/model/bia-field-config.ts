import { Validator } from "@angular/forms";

export enum PrimeNGFiltering {
  StartsWith = 'startsWith',
  Contains = 'contains',
  EndsWith = 'endsWith',
  Equals = 'equals',
  NotEquals = 'notEquals',
  In = 'in',
  Lt = 'lt',
  Lte = 'lte',
  Gt = 'gt',
  Gte = 'gte'
}

export enum PropType {
  Date = 'Date',
  DateTime = 'DateTime',
  Time = 'Time', // For dateTime field if you just manipulate Time
  TimeOnly = 'TimeOnly',
  TimeSecOnly = 'TimeSecOnly',
  Number = 'Number',
  Boolean = 'Boolean',
  String = 'String',
  OneToMany = 'OneToMany',
  ManyToMany = 'ManyToMany'
}

export class BiaFieldConfig {
  field: string;
  header: string;
  type: PropType;
  filterMode: PrimeNGFiltering;
  formatDate: string;
  primeDateFormat: string;
  hourFormat: number;
  isSearchable: boolean;
  isSortable: boolean;
  icon: string;
  isEditable: boolean;
  maxlength: number;
  translateKey: string;
  searchPlaceholder: string;
  isRequired: boolean;
  validators: Validator[];
  specificOutput: boolean;
  specificInput: boolean;
  get isDate() {
    return this.type === PropType.Date || this.type === PropType.DateTime || this.type === PropType.Time;
  }
  get filterPlaceHolder() {
    if (this.searchPlaceholder !== undefined) {
      return this.searchPlaceholder;
    }
    return this.isDate === true ? 'bia.dateIso8601' : '';
  }

  constructor(field: string, header: string, maxlength = 255) {
    this.field = field;
    this.header = header;
    this.type = PropType.String;
    this.filterMode = PrimeNGFiltering.Contains;
    this.formatDate = '';
    this.primeDateFormat = 'yy/mm/dd';
    this.hourFormat = 12;
    this.isSearchable = true;
    this.isSortable = true;
    this.icon = '';
    this.isEditable = true;
    this.maxlength = maxlength;
    this.isRequired = false;
    this.specificOutput = false;
    this.specificInput = false;
    this.validators = [];
  }

  public clone(): BiaFieldConfig {
    return Object.assign(new BiaFieldConfig(this.field, this.header, this.maxlength), {
      type: this.type,
      filterMode: this.filterMode,
      formatDate: this.formatDate,
      primeDateFormat: this.primeDateFormat,
      hourFormat: this.hourFormat,
      isSearchable: this.isSearchable,
      isSortable: this.isSortable,
      icon: this.icon,
      isEditable: this.isEditable,
      translateKey: this.translateKey,
      searchPlaceholder: this.searchPlaceholder,
      isRequired: this.isRequired,
      specificOutput: this.specificOutput,
      specificInput: this.specificInput,
      validators: this.validators,
    })
  }
}

export interface BiaFieldsConfig {
  columns: BiaFieldConfig[];
  advancedFilter?: any;
}
