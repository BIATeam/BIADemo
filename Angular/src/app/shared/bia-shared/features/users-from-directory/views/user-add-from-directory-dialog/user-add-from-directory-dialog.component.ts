import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { Observable } from 'rxjs';
import { DomaineUsersFromDirectoryActions } from '../../store/users-from-Directory-actions';
import { getAllUsersFromDirectory } from '../../store/user-from-Directory.state';
import { getAllLdapDomain } from 'src/app/domains/ldap-domain/store/ldap-domain.state';
import { UserFromDirectory } from 'src/app/shared/bia-shared/features/users-from-directory/model/user-from-Directory';
import { LdapDomain } from 'src/app/domains/ldap-domain/model/ldap-domain';
import { DomaineLdapActions } from 'src/app/domains/ldap-domain/store/ldap-domain-actions';
import { UserFilter } from 'src/app/shared/bia-shared/features/users-from-directory/model/user-filter';

@Component({
  selector: 'bia-user-add-from-directory-dialog',
  templateUrl: './user-add-from-directory-dialog.component.html',
  styleUrls: ['./user-add-from-directory-dialog.component.scss']
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
    this.usersFromDirectory$ = this.store.select(getAllUsersFromDirectory);

    this.ldapDomains$ = this.store.select(getAllLdapDomain);

    this.store.dispatch(DomaineLdapActions.loadAll());
  }

  onSubmitted(userToCreates: UserFromDirectory[]) {
    this.store.dispatch(DomaineUsersFromDirectoryActions.addFromDirectory({ usersFromDirectory: userToCreates }));
    this.close();
  }

  onCancelled() {
    this.close();
  }

  public close() {
    this.display = false;
  }

  onSearchUsers(userFilter: UserFilter) {
    this.store.dispatch(DomaineUsersFromDirectoryActions.loadAllByFilter({ userFilter }));
  }
}
