export class TeamAdvancedFilterDto {
  userId: number;

  static hasFilter(filter: TeamAdvancedFilterDto): boolean {
    return filter?.userId != null;
  }
}
