import { BiaFieldConfig } from './bia-field-config';

export class BiaFormLayoutConfig<TDto> {
  constructor(public items: BiaFormLayoutConfigItem<TDto>[] = []) {}
}

export type BiaFormLayoutConfigItem<TDto> =
  | BiaFormLayoutConfigGroup<TDto>
  | BiaFormLayoutConfigRow<TDto>;

export class BiaFormLayoutConfigGroup<TDto> {
  readonly type = 'group';
  constructor(
    public name: string,
    public rows: BiaFormLayoutConfigRow<TDto>[]
  ) {}
}

export class BiaFormLayoutConfigRow<TDto> {
  readonly type = 'row';
  constructor(public columns: BiaFormLayoutConfigColumn<TDto>[]) {}

  get columnClass(): string {
    const columnCount = this.columns.length;
    const columnSize = columnCount > 0 ? Math.floor(12 / columnCount) : 12;
    return `col-${columnSize}`;
  }
}

export class BiaFormLayoutConfigColumn<TDto> {
  fieldConfig: BiaFieldConfig<TDto>;

  constructor(public field: keyof TDto & string) {}
}
