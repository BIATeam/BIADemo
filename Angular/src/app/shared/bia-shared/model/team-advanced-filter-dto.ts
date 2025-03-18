export class TeamAdvancedFilterDto {
  userId: number;

  static hasFilter(filter: TeamAdvancedFilterDto): boolean {
    return !!filter && !!filter.userId;
  }
}
