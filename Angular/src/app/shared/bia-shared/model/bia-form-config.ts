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
}

export class BiaFormConfigColumn<TDto> {
  constructor(public field: keyof TDto & string) {}
}
