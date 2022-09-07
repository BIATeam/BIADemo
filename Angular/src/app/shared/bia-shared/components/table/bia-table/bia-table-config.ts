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

export interface CustomButton {
  classValue: string;
  numEvent: number;
  pTooltipValue: string;
  permission: string;
}

export class PrimeTableColumn {
  field: string;
  header: string;
  type: PropType;
  filterMode: PrimeNGFiltering;
  formatDate: string;
  isSearchable: boolean;
  isSortable: boolean;
  isEditable: boolean;
  maxlength: number;
  translateKey: string;
  searchPlaceholder: string;
  isRequired: boolean;
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
    this.isSearchable = true;
    this.isSortable = true;
    this.isEditable = true;
    this.maxlength = maxlength;
    this.isRequired = false;
    this.specificOutput = false;
    this.specificInput = false;
  }
  
  public clone() : PrimeTableColumn
  {
    return Object.assign(new PrimeTableColumn(this.field,this.header,this.maxlength), {
      type : this.type,
      filterMode : this.filterMode,
      formatDate : this.formatDate,
      isSearchable : this.isSearchable,
      isSortable : this.isSortable,
      isEditable : this.isEditable,
      translateKey: this.translateKey,
      searchPlaceholder: this.searchPlaceholder,
      isRequired: this.isRequired,
      specificOutput: this.specificOutput,
      specificInput: this.specificInput,
    })
  }
}

export interface BiaListConfig {
  columns: PrimeTableColumn[];
}
