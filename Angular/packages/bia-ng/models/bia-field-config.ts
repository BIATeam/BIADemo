import { ValidatorFn } from '@angular/forms';
import {
  FieldEditMode,
  NumberMode,
  PrimeNGFiltering,
  PropType,
  TableColumnVisibility,
} from 'packages/bia-ng/models/enum/public-api';
import { FilterMetadata } from 'primeng/api';

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
    this.mode = NumberMode.Decimal;
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

export class BiaFieldMultilineString {
  rows: number;
  cols: number;
  resize: boolean;
  constructor() {
    this.rows = 5;
    this.cols = 30;
    this.resize = false;
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
  fieldEditMode: FieldEditMode;
  isEditableChoice: boolean;
  isVisibleInForm: boolean;
  tableColumnVisibility: TableColumnVisibility;
  maxlength: number;
  translateKey: string;
  searchPlaceholder: string;
  isRequired: boolean;
  validators: ValidatorFn[];
  specificOutput: boolean;
  specificInput: boolean;
  minWidth: string;
  maxWidth: string;
  isFrozen: boolean;
  alignFrozen: string;
  displayFormat: BiaFieldNumberFormat | BiaFieldDateFormat | null;
  maxConstraints = 10;
  filterWithDisplay: boolean;
  customDisplayFormat: boolean = true;
  multiline?: BiaFieldMultilineString;
  asLocalDateTime: boolean = false;
  allowSelectFilter: boolean;
  defaultFilter?: FilterMetadata | FilterMetadata[];

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

  /** True when the field is editable in at least one form mode. */
  get isEditable(): boolean {
    return this.fieldEditMode !== FieldEditMode.ReadOnly;
  }

  /** True when the field is editable only on create (disabled on edit). */
  get isOnlyInitializable(): boolean {
    return this.fieldEditMode === FieldEditMode.InitializableOnly;
  }

  /** True when the field is editable only on update (disabled on create). */
  get isOnlyUpdatable(): boolean {
    return this.fieldEditMode === FieldEditMode.UpdatableOnly;
  }

  get isVisibleInTable(): boolean {
    return this.tableColumnVisibility !== TableColumnVisibility.Hidden;
  }

  get isHiddenByDefault(): boolean {
    return (
      this.tableColumnVisibility === TableColumnVisibility.AvailableButHidden
    );
  }

  constructor(field: keyof TDto & string, header: string, maxlength = 255) {
    this.field = field;
    this.header = header;
    this.type = PropType.String;
    this.filterMode = PrimeNGFiltering.Contains;
    this.isSearchable = true;
    this.isSortable = true;
    this.icon = '';
    this.fieldEditMode = FieldEditMode.Editable;
    this.isEditableChoice = false;
    this.isVisibleInForm = true;
    this.tableColumnVisibility = TableColumnVisibility.Visible;
    this.maxlength = maxlength;
    this.isRequired = false;
    this.specificOutput = false;
    this.specificInput = false;
    this.validators = [];
    this.minWidth = '';
    this.maxWidth = '';
    this.isFrozen = false;
    this.alignFrozen = 'left';
    this.displayFormat = null;
    this.filterWithDisplay = false;
    this.multiline = undefined;
    this.asLocalDateTime = false;
    this.allowSelectFilter = false;
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
        fieldEditMode: this.fieldEditMode,
        isChoiceEditable: this.isEditableChoice,
        isVisibleInForm: this.isVisibleInForm,
        tableColumnVisibility: this.tableColumnVisibility,
        translateKey: this.translateKey,
        searchPlaceholder: this.searchPlaceholder,
        isRequired: this.isRequired,
        specificOutput: this.specificOutput,
        specificInput: this.specificInput,
        validators: this.validators,
        minWidth: this.minWidth,
        maxWidth: this.maxWidth,
        isFrozen: this.isFrozen,
        alignFrozen: this.alignFrozen,
        displayFormat: this.displayFormat,
        filterWithDisplay: this.filterWithDisplay,
        customDisplayFormat: this.customDisplayFormat,
        multiline: this.multiline,
        asLocalDateTime: this.asLocalDateTime,
        allowSelectFilter: this.allowSelectFilter,
        defaultFilter: this.defaultFilter,
      }
    );
  }
}

export interface BiaFieldsConfig<TDto> {
  columns: BiaFieldConfig<TDto>[];
  formValidators?: ValidatorFn[];
  advancedFilter?: any;
}
