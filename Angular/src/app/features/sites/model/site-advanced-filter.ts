export class SiteAdvancedFilter {
  userId: number;

  static haveFilter(filter: SiteAdvancedFilter) : boolean{
    return filter?.userId != null
  }
}
