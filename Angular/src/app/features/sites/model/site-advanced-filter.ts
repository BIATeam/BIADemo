export class SiteAdvancedFilter {
  userId: number;

  static hasFilter(filter: SiteAdvancedFilter) : boolean{
    return filter?.userId != null
  }
}
