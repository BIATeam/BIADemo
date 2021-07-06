import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Store } from '@ngrx/store';
import { save } from '../../store/users-actions';
import { AppState } from 'src/app/store/state';
import { Observable } from 'rxjs';
import { loadAllByFilter } from 'src/app/domains/user-from-AD/store/users-from-AD-actions';
import { getAllUsersFromAD } from 'src/app/domains/user-from-AD/store/user-from-AD.state';
import { getAllLdapDomain } from 'src/app/domains/ldap-domain/store/ldap-domain.state';
import { UserFromAD } from 'src/app/domains/user-from-AD/model/user-from-AD';
import { LdapDomain } from 'src/app/domains/ldap-domain/model/ldap-domain';
import { loadAll as loadAllLdapDomains } from 'src/app/domains/ldap-domain/store/ldap-domain-actions';
import { UserFilter } from '../../model/UserFilter';

@Component({
  selector: 'app-user-new-dialog',
  templateUrl: './user-new-dialog.component.html',
  styleUrls: ['./user-new-dialog.component.scss']
})
export class UserNewDialogComponent implements OnInit {
  _display = false;

  @Input()
  set display(val: boolean) {
    this._display = val;
    this.displayChange.emit(this._display);
  }

  @Output() displayChange = new EventEmitter<boolean>();
  usersFromAD$: Observable<UserFromAD[]>;
  ldapDomains$: Observable<LdapDomain[]>;

  constructor(private store: Store<AppState>) {}

  async ngOnInit() {
    this.usersFromAD$ = this.store.select(getAllUsersFromAD).pipe();

    this.ldapDomains$ = this.store.select(getAllLdapDomain).pipe();

    this.store.dispatch(loadAllLdapDomains());
  }

  onSubmitted(userToCreates: UserFromAD[]) {
    this.store.dispatch(save({ usersFromAD: userToCreates }));
    this.close();
  }

  onCancelled() {
    this.close();
  }

  public close() {
    this.display = false;
  }

  onSearchUsers(userFilter: UserFilter) {
    this.store.dispatch(loadAllByFilter({ userFilter }));
  }
}
