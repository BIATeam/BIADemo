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

  get computedColumnClass(): string {
    const definedLgSizes = this.columns
      .filter(c => c.isLgSizeValid)
      .map(c => c.lgSize as number);
    const totalDefinedLgSize = definedLgSizes.reduce(
      (acc, size) => acc + size,
      0
    );
    const remainingLgSize = 12 - totalDefinedLgSize;

    const undefinedLgSizeColumns = this.columns.length - definedLgSizes.length;
    const lgSize = Math.floor(remainingLgSize / undefinedLgSizeColumns);
    return generateColumnClassFromLgSize(lgSize);
  }
}

export class BiaFormLayoutConfigColumn<TDto> {
  fieldConfig: BiaFieldConfig<TDto>;

  constructor(
    public field: keyof TDto & string,
    public columnSize?: number | BiaFormLayoutConfigColumnSize | undefined
  ) {}

  get isLgSizeValid(): boolean {
    return this.lgSize !== undefined && this.lgSize >= 1 && this.lgSize <= 12;
  }

  get lgSize(): number | undefined {
    if (!this.columnSize) {
      return undefined;
    }

    if (this.columnSize instanceof BiaFormLayoutConfigColumnSize) {
      return (this.columnSize as BiaFormLayoutConfigColumnSize).lgSize;
    }

    return this.columnSize as number;
  }

  get columnClass(): string | undefined {
    if (this.columnSize && this.isLgSizeValid) {
      if (this.columnSize instanceof BiaFormLayoutConfigColumnSize) {
        return generateColumnClassFromColumnSize(
          this.columnSize as BiaFormLayoutConfigColumnSize
        );
      }

      return generateColumnClassFromLgSize(this.columnSize as number);
    }

    return undefined;
  }
}

export class BiaFormLayoutConfigColumnSize {
  constructor(
    public lgSize: number,
    public mdSize: number,
    public smSize: number,
    public mobileFirstSize: number
  ) {}
}

function generateColumnClassFromLgSize(lgSize: number): string {
  const mdSize = Math.min(12, Math.ceil(lgSize * 1.5));
  const smSize = Math.min(12, Math.ceil(lgSize * 2));
  return `col-12 lg:col-${lgSize} md:col-${mdSize} sm:col-${smSize}`;
}

function generateColumnClassFromColumnSize(
  columnSize: BiaFormLayoutConfigColumnSize
): string {
  return `col-${columnSize.mobileFirstSize} lg:col-${columnSize.lgSize} md:col-${columnSize.mdSize} sm:col-${columnSize.smSize}`;
}
