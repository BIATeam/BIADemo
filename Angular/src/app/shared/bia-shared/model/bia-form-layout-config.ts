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

  get defaultColumnClass(): string {
    const definedSizes = this.columns
      .filter(c => c.isColumnSizeValid)
      .map(c => c.columnSize as number);
    const totalDefinedSize = definedSizes.reduce((acc, size) => acc + size, 0);
    const remainingSize = 12 - totalDefinedSize;

    const undefinedSizeColumns = this.columns.length - definedSizes.length;
    const defaultSize = Math.floor(remainingSize / undefinedSizeColumns);

    return `col-${defaultSize}`;
  }
}

export class BiaFormLayoutConfigColumn<TDto> {
  fieldConfig: BiaFieldConfig<TDto>;

  constructor(
    public field: keyof TDto & string,
    public columnSize?: number | undefined
  ) {}

  get isColumnSizeValid(): boolean {
    return (
      this.columnSize !== undefined &&
      this.columnSize >= 1 &&
      this.columnSize <= 12
    );
  }

  get columnClass(): string | undefined {
    return this.isColumnSizeValid ? `col-${this.columnSize}` : undefined;
  }
}
