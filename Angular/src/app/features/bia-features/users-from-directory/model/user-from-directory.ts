export interface UserFromDirectory {
  sid: string;
  lastName: string;
  firstName: string;
  login: string;
  domain: string;
  guid: string;
  email: string;
  distinguishedName: string;
  isEmployee: boolean;
  isExternal: boolean;
  externalCompany: string;
  company: string;
  site: string;
  manager: string;
  department: string;
  subDepartment: string;
  office: string;
  country: string;
  // Computed by Angular when data receive
  displayName: string;
}