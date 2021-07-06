export interface User {
  id: number;
  lastName: string;
  firstName: string;
  login: string;
  guid: string;
  siteIds: number[];
  // Computed by Angular when data receive
  displayName: string;
}
