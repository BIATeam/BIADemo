import { BiaFieldConfig } from './bia-field-config';

export class BiaFormLayoutConfig<TDto> {
  constructor(
    public items: BiaFormLayoutConfigItem<TDto>[] = [],
    public autoFocusFirstField: boolean = true
  ) {}
}

export type BiaFormLayoutConfigItem<TDto> =
  | BiaFormLayoutConfigGroup<TDto>
  | BiaFormLayoutConfigRow<TDto>
  | BiaFormLayoutConfigTabGroup<TDto>;

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
    return generateColumnClassFromLgSize(lgSize, this.columns.length);
  }
}

export abstract class BiaFormLayoutConfigColumn<TDto> {
  _element: keyof TDto;

  constructor(
    public columnSize?: number | BiaFormLayoutConfigColumnSize | undefined
  ) {}

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
}

export class BiaFormLayoutConfigGroup<
  TDto,
> extends BiaFormLayoutConfigColumn<TDto> {
  readonly type = 'group';
  constructor(
    public name: string,
    public rows: BiaFormLayoutConfigRow<TDto>[],
    public columnSize?: number | BiaFormLayoutConfigColumnSize | undefined
  ) {
    super(columnSize);
  }
}

export class BiaFormLayoutConfigField<
  TDto,
> extends BiaFormLayoutConfigColumn<TDto> {
  readonly type = 'field';
  fieldConfig: BiaFieldConfig<TDto>;

  constructor(
    public field: keyof TDto & string,
    public columnSize?: number | BiaFormLayoutConfigColumnSize | undefined
  ) {
    super(columnSize);
  }
}

export class BiaFormLayoutConfigTabGroup<
  TDto,
> extends BiaFormLayoutConfigColumn<TDto> {
  readonly type = 'tab';

  constructor(
    public tabs: BiaFormLayoutConfigTab<TDto>[],
    public columnSize?: number | BiaFormLayoutConfigColumnSize | undefined
  ) {
    super(columnSize);
  }
}

export class BiaFormLayoutConfigTab<TDto> {
  constructor(
    public id: string,
    public name: string,
    public items: BiaFormLayoutConfigItem<TDto>[]
  ) {}
}

export class BiaFormLayoutConfigColumnSize {
  constructor(
    public lgSize: number,
    public mdSize: number,
    public smSize: number,
    public mobileFirstSize: number
  ) {}
}

/**
 * Snaps a raw size up to the nearest clean grid division of 12.
 * Valid divisions: 1, 2, 3, 4, 6, 12
 */
function snapToGrid(size: number): number {
  const gridDivisions = [1, 2, 3, 4, 6, 12];
  return gridDivisions.find(d => d >= size) ?? 12;
}

/**
 * Generates responsive column classes.
 * md: fits at most 2 columns per row (col-6 each), or full-width if only 1 column.
 * sm: always full-width.
 */
function generateColumnClassFromLgSize(lgSize: number, colsInRow = 1): string {
  // const mdColsPerRow = colsInRow <= 1 ? 1 : 2;
  const mdSize = snapToGrid(Math.ceil(12 / colsInRow));
  const smSize = 12;
  return `col-12 lg:col-${lgSize} md:col-${mdSize} sm:col-${smSize}`;
}

function generateColumnClassFromColumnSize(
  columnSize: BiaFormLayoutConfigColumnSize
): string {
  return `col-${columnSize.mobileFirstSize} lg:col-${columnSize.lgSize} md:col-${columnSize.mdSize} sm:col-${columnSize.smSize}`;
}
