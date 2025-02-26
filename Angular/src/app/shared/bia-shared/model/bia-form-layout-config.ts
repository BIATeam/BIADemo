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
    return generateColumnClass(lgSize);
  }
}

export class BiaFormLayoutConfigColumn<TDto> {
  fieldConfig: BiaFieldConfig<TDto>;

  constructor(
    public field: keyof TDto & string,
    public lgSize?: number | undefined
  ) {}

  get isLgSizeValid(): boolean {
    return this.lgSize !== undefined && this.lgSize >= 1 && this.lgSize <= 12;
  }

  get columnClass(): string | undefined {
    if (this.isLgSizeValid) {
      return generateColumnClass(this.lgSize as number);
    }

    return undefined;
  }
}

function generateColumnClass(lgSize: number): string {
  const mdSize = Math.min(12, Math.ceil(lgSize * 1.5));
  const smSize = Math.min(12, Math.ceil(lgSize * 2));
  return `col-12 lg:col-${lgSize} md:col-${mdSize} sm:col-${smSize}`;
}
