export class PlaneAdvancedFilterDto {
  enginesNumberRange: number;

  static hasFilter(filter: PlaneAdvancedFilterDto): boolean {
    return !!filter && !!filter.enginesNumberRange;
  }
}
