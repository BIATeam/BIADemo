export interface UserFromDirectory {
  lastName: string;
  firstName: string;
  login: string;
  domain: string;
  guid: string;
  sid: string;
  // Computed by Angular when data receive
  displayName: string;
}