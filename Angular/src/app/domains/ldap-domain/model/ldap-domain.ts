export interface LdapDomain {
  name: string;
  ldapName: string;
  ldapServiceAccount: string;
  ldapServicePass: string;
  credentialKeyInWindowsVault: string;
  containsGroup: boolean;
  containsUser: boolean;
  // Computed by Angular when data received
  displayName: string;
}
