export interface UserFromAD {
  lastName: string;
  firstName: string;
  login: string;
  domain: string;
  guid: string;
  // Computed by Angular when data receive
  displayName: string;
}
