export class BiaFormConfig<TDto> {
  groups: BiaFormConfigGroup<TDto>[];
  rows: BiaFormConfigRow<TDto>[];
}

export class BiaFormConfigGroup<TDto> {
  name: string;
  rows: BiaFormConfigRow<TDto>[];
}

export class BiaFormConfigRow<TDto> {
  columns: BiaFormConfigColumn<TDto>[];
}

export class BiaFormConfigColumn<TDto> {
  field: keyof TDto & string;
}
