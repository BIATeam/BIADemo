import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges
} from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { LdapDomain } from 'src/app/domains/bia-domains/ldap-domain/model/ldap-domain';
import { UserFromDirectory } from '../../model/user-from-directory';
import { UserFilter } from '../../model/user-filter';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';

@Component({
  selector: 'bia-user-from-directory-form',
  templateUrl: './user-from-directory-form.component.html',
  styleUrls: ['./user-from-directory-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserFromLdapFormComponent implements OnChanges {
  @Output() searchUsers = new EventEmitter<UserFilter>();
  @Output() save = new EventEmitter<UserFromDirectory[]>();
  @Output() cancel = new EventEmitter<void>();
  @Input() users: UserFromDirectory[];
  @Input() domains: LdapDomain[];
  @Input() returnSizeOptions: number[] = [10, 25, 50, 100];

  selectedUsers: UserFromDirectory[];
  selectedDomain: string;
  form: UntypedFormGroup;
  useKeycloak = false;

  constructor(public formBuilder: UntypedFormBuilder, appSettingsService: AppSettingsService) {
    this.initForm();
    this.useKeycloak = appSettingsService.appSettings?.keycloak?.isActive;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.users && changes.users) {
      this.users = this.users.sort((a, b) => {
        return a.displayName.localeCompare(b.displayName);
      });
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      selectedUsers: [this.selectedUsers, Validators.required],
      domains: [this.domains],
      returnSize: this.returnSizeOptions[0]
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
    this.form.reset({returnSize:this.returnSizeOptions[0]});
  }

  onSearchUsers(event: any) {
    const userFiter: UserFilter = {
      filter: event.query,
      ldapName: this.selectedDomain,
      returnSize: this.form.value.returnSize ? this.form.value.returnSize : this.returnSizeOptions[0]
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
