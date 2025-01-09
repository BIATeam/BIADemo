/* eslint-disable @typescript-eslint/naming-convention */
import { Validator, ValidatorFn } from '@angular/forms';

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
  Gte = 'gte',
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
  ManyToMany = 'ManyToMany',
}

export enum NumberMode {
  Default = 'decimal',
  Decimal = 'decimal',
  Currency = 'currency',
}

export class BiaFieldNumberFormat {
  autoLocale: string; // property automaticaly set when culture change.
  mode: NumberMode; // can be default, decimal, currency
  currency: string; // can be USD(default), EUR ...
  currencyDisplay: string; // can be symbole(default), code
  minFractionDigits: number | null; // can be null(default) or an integer
  maxFractionDigits: number | null; // can be null(default) or an integer
  min: number | null;
  max: number | null;
  constructor() {
    this.autoLocale = '';
    this.mode = NumberMode.Default;
    this.currency = 'USD';
    this.currencyDisplay = 'symbol';
    this.minFractionDigits = null;
    this.maxFractionDigits = null;
    this.min = null;
    this.max = null;
  }

  get outputFormat(): string {
    return (
      '1.' +
      (this.minFractionDigits || 0).toString() +
      '-' +
      (this.maxFractionDigits || 0).toString()
    );
  }
}

export class BiaFieldDateFormat {
  autoFormatDate: string; // property automaticaly set when culture change.
  autoPrimeDateFormat: string; // property automaticaly set when culture change.
  autoHourFormat: number; // property automaticaly set when culture change.
  constructor() {
    this.autoFormatDate = '';
    this.autoPrimeDateFormat = 'yy/mm/dd';
    this.autoHourFormat = 12;
  }
}

export class BiaFieldConfig<TDto> {
  field: keyof TDto & string;
  header: string;
  type: PropType;
  filterMode: PrimeNGFiltering;
  isSearchable: boolean;
  isSortable: boolean;
  icon: string;
  isEditable: boolean;
  isOnlyInitializable: boolean;
  isOnlyUpdatable: boolean;
  isEditableChoice: boolean;
  isVisible: boolean;
  isHideByDefault: boolean;
  maxlength: number;
  translateKey: string;
  searchPlaceholder: string;
  isRequired: boolean;
  validators: Validator[];
  specificOutput: boolean;
  specificInput: boolean;
  minWidth: string;
  isFrozen: boolean;
  alignFrozen: string;
  displayFormat: BiaFieldNumberFormat | BiaFieldDateFormat | null;
  maxConstraints = 10;

  get isDate() {
    return (
      this.type === PropType.Date ||
      this.type === PropType.DateTime ||
      this.type === PropType.Time
    );
  }
  get filterPlaceHolder() {
    if (this.searchPlaceholder !== undefined) {
      return this.searchPlaceholder;
    }
    return this.isDate === true ? 'bia.dateIso8601' : '';
  }
  get typeLowerCase() {
    return this.type.toLowerCase();
  }

  constructor(field: keyof TDto & string, header: string, maxlength = 255) {
    this.field = field;
    this.header = header;
    this.type = PropType.String;
    this.filterMode = PrimeNGFiltering.Contains;
    this.isSearchable = true;
    this.isSortable = true;
    this.icon = '';
    this.isEditable = true;
    this.isOnlyInitializable = false;
    this.isOnlyUpdatable = false;
    this.isEditableChoice = false;
    this.isVisible = true;
    this.isHideByDefault = false;
    this.maxlength = maxlength;
    this.isRequired = false;
    this.specificOutput = false;
    this.specificInput = false;
    this.validators = [];
    this.minWidth = '';
    this.isFrozen = false;
    this.alignFrozen = 'left';
    this.displayFormat = null;
  }

  public clone(): BiaFieldConfig<TDto> {
    return Object.assign(
      new BiaFieldConfig<TDto>(this.field, this.header, this.maxlength),
      {
        type: this.type,
        filterMode: this.filterMode,
        isSearchable: this.isSearchable,
        isSortable: this.isSortable,
        icon: this.icon,
        isEditable: this.isEditable,
        isOnlyInitializable: this.isOnlyInitializable,
        isOnlyUpdatable: this.isOnlyUpdatable,
        isChoiceEditable: this.isEditableChoice,
        isVisible: this.isVisible,
        isHideByDefault: this.isHideByDefault,
        translateKey: this.translateKey,
        searchPlaceholder: this.searchPlaceholder,
        isRequired: this.isRequired,
        specificOutput: this.specificOutput,
        specificInput: this.specificInput,
        validators: this.validators,
        minWidth: this.minWidth,
        isFrozen: this.isFrozen,
        alignFrozen: this.alignFrozen,
        displayFormat: this.displayFormat,
      }
    );
  }
}

export interface BiaFieldsConfig<TDto> {
  columns: BiaFieldConfig<TDto>[];
  formValidators?: ValidatorFn[];
  advancedFilter?: any;
}
