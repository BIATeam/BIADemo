import { BiaFieldConfig } from './bia-field-config';

export interface BiaFormConfig<TDto> {
  groups?: BiaFormConfigGroup<TDto>[];
  rows?: BiaFormConfigRow<TDto>[];
}

export class BiaFormConfigGroup<TDto> {
  constructor(
    public name: string,
    public rows: BiaFormConfigRow<TDto>[]
  ) {}
}

export class BiaFormConfigRow<TDto> {
  constructor(public columns: BiaFormConfigColumn<TDto>[]) {}

  get columnClass(): string {
    const columnCount = this.columns.length;
    const columnSize = columnCount > 0 ? Math.floor(12 / columnCount) : 12;
    return `col-${columnSize}`;
  }
}

export class BiaFormConfigColumn<TDto> {
  fieldConfig: BiaFieldConfig<TDto>;

  constructor(public field: keyof TDto & string) {}
}
