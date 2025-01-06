import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { LdapDomain } from 'src/app/domains/bia-domains/ldap-domain/model/ldap-domain';
import { DomainLdapDomainsActions } from 'src/app/domains/bia-domains/ldap-domain/store/ldap-domain-actions';
import { getAllLdapDomain } from 'src/app/domains/bia-domains/ldap-domain/store/ldap-domain.state';
import { AppState } from 'src/app/store/state';
import { UserFilter } from '../../model/user-filter';
import { UserFromDirectory } from '../../model/user-from-directory';
import { getAllUsersFromDirectory } from '../../store/user-from-directory.state';
import { FeatureUsersFromDirectoryActions } from '../../store/users-from-directory-actions';

@Component({
  selector: 'bia-user-add-from-directory-dialog',
  templateUrl: './user-add-from-directory-dialog.component.html',
  styleUrls: ['./user-add-from-directory-dialog.component.scss'],
})
export class UserAddFromLdapComponent implements OnInit {
  _display = false;

  @Input()
  set display(val: boolean) {
    this._display = val;
    this.displayChange.emit(this._display);
  }

  @Output() displayChange = new EventEmitter<boolean>();
  usersFromDirectory$: Observable<UserFromDirectory[]>;
  ldapDomains$: Observable<LdapDomain[]>;

  constructor(protected store: Store<AppState>) {}

  async ngOnInit() {
    this.usersFromDirectory$ = this.store.select(getAllUsersFromDirectory);

    this.ldapDomains$ = this.store.select(getAllLdapDomain);

    this.store.dispatch(DomainLdapDomainsActions.loadAll());
  }

  onSubmitted(userToCreates: UserFromDirectory[]) {
    this.store.dispatch(
      FeatureUsersFromDirectoryActions.addFromDirectory({
        usersFromDirectory: userToCreates,
      })
    );
    this.close();
  }

  onCancelled() {
    this.close();
  }

  public close() {
    this.display = false;
  }

  onSearchUsers(userFilter: UserFilter) {
    this.store.dispatch(
      FeatureUsersFromDirectoryActions.loadAllByFilter({ userFilter })
    );
  }
}
