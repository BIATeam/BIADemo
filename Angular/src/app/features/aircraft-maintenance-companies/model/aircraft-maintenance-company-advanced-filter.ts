export class AircraftMaintenanceCompanyAdvancedFilter {
  userId: number;

  static hasFilter(filter: AircraftMaintenanceCompanyAdvancedFilter) : boolean{
    return filter?.userId != null
  }
}
