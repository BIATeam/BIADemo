import { AsyncPipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import {
  DomainLdapDomainsActions,
  DomainLdapDomainsStore,
} from 'packages/bia-ng/domains/public-api';
import { LdapDomain } from 'packages/bia-ng/models/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { SharedModule } from 'primeng/api';
import { Dialog } from 'primeng/dialog';
import { Observable } from 'rxjs';
import { UserFromLdapFormComponent } from '../../components/user-from-directory-form/user-from-directory-form.component';
import { UserFilter } from '../../model/user-filter';
import { UserFromDirectory } from '../../model/user-from-directory';
import { UsersFromDirectoryStore } from '../../store/user-from-directory.state';
import { FeatureUsersFromDirectoryActions } from '../../store/users-from-directory-actions';

@Component({
  selector: 'bia-user-add-from-directory-dialog',
  templateUrl: './user-add-from-directory-dialog.component.html',
  styleUrls: ['./user-add-from-directory-dialog.component.scss'],
  imports: [
    Dialog,
    SharedModule,
    UserFromLdapFormComponent,
    AsyncPipe,
    TranslateModule,
  ],
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

  constructor(protected store: Store<BiaAppState>) {}

  async ngOnInit() {
    this.usersFromDirectory$ = this.store.select(
      UsersFromDirectoryStore.getAllUsersFromDirectory
    );

    this.ldapDomains$ = this.store.select(
      DomainLdapDomainsStore.getAllLdapDomain
    );

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
