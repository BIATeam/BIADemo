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
  Time = 'Time',
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
  get isDate() {
    return this.type === PropType.Date || this.type === PropType.DateTime || this.type === PropType.Time;
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
  }
}

export interface BiaListConfig {
  columns: PrimeTableColumn[];
}
