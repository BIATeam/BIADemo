import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { Observable } from 'rxjs';
import { DomaineUsersFromADActions } from 'src/app/domains/user-from-AD/store/users-from-AD-actions';
import { getAllUsersFromAD } from 'src/app/domains/user-from-AD/store/user-from-AD.state';
import { getAllLdapDomain } from 'src/app/domains/ldap-domain/store/ldap-domain.state';
import { UserFromDirectory } from 'src/app/domains/user-from-AD/model/user-from-AD';
import { LdapDomain } from 'src/app/domains/ldap-domain/model/ldap-domain';
import { DomaineLdapActions } from 'src/app/domains/ldap-domain/store/ldap-domain-actions';
import { UserFilter } from 'src/app/domains/user-from-AD/model/user-filter';

@Component({
  selector: 'bia-user-add-from-ldap-dialog',
  templateUrl: './user-add-from-ldap-dialog.component.html',
  styleUrls: ['./user-add-from-ldap-dialog.component.scss']
})
export class UserAddFormLdapComponent implements OnInit {
  _display = false;

  @Input()
  set display(val: boolean) {
    this._display = val;
    this.displayChange.emit(this._display);
  }

  @Output() displayChange = new EventEmitter<boolean>();
  usersFromDirectory$: Observable<UserFromDirectory[]>;
  ldapDomains$: Observable<LdapDomain[]>;

  constructor(private store: Store<AppState>) {}

  async ngOnInit() {
    this.usersFromDirectory$ = this.store.select(getAllUsersFromAD);

    this.ldapDomains$ = this.store.select(getAllLdapDomain);

    this.store.dispatch(DomaineLdapActions.loadAll());
  }

  onSubmitted(userToCreates: UserFromDirectory[]) {
    this.store.dispatch(DomaineUsersFromADActions.addFromDirectory({ usersFromDirectory: userToCreates }));
    this.close();
  }

  onCancelled() {
    this.close();
  }

  public close() {
    this.display = false;
  }

  onSearchUsers(userFilter: UserFilter) {
    this.store.dispatch(DomaineUsersFromADActions.loadAllByFilter({ userFilter }));
  }
}
