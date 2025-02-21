import { BiaFieldConfig } from './bia-field-config';

export class BiaFormConfig<TDto> {
  constructor(public config: BiaFormConfigItem<TDto>[] = []) {}
}

export type BiaFormConfigItem<TDto> =
  | BiaFormConfigGroup<TDto>
  | BiaFormConfigRow<TDto>;

export class BiaFormConfigGroup<TDto> {
  readonly type = 'group';
  constructor(
    public name: string,
    public rows: BiaFormConfigRow<TDto>[]
  ) {}
}

export class BiaFormConfigRow<TDto> {
  readonly type = 'row';
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
