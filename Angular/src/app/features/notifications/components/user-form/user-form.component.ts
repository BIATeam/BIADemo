import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LdapDomain } from 'src/app/domains/ldap-domain/model/ldap-domain';
import { User } from 'src/app/domains/user/model/user';
import { UserFilter } from '../../model/UserFilter';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserFormComponent implements OnInit, OnChanges {
  @Output() searchUsers = new EventEmitter<UserFilter>();
  @Output() save = new EventEmitter<User[]>();
  @Output() cancel = new EventEmitter();
  @Input() users: User[];
  @Input() domains: LdapDomain[];

  selectedUsers: User[];
  selectedDomain: string;
  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.initForm();
  }

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (this.users && changes.users) {
      this.users = this.users.sort((a, b) => {
        return a.firstName.localeCompare(b.firstName);
      });
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      selectedUsers: [this.selectedUsers, Validators.required],
      domains: [this.domains]
    });
  }

  onCancel() {
    this.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      this.save.emit(this.form.value.selectedUsers);
      this.reset();
    }
  }

  reset() {
    this.selectedDomain = '';
    this.form.reset();
  }

  onSearchUsers(event: any) {
    const userFiter: UserFilter = {
      filter: event.query,
      ldapName: this.selectedDomain
    };
    this.searchUsers.emit(userFiter);
  }

  onDomainChange(event: any) {
    const domain = event.value as LdapDomain;
    if (domain) {
      this.selectedDomain = (event.value as LdapDomain).ldapName;
    } else {
      this.selectedDomain = '';
    }
  }
}
