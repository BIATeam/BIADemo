import { NgIf } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ButtonDirective } from 'primeng/button';
import { FloatLabel } from 'primeng/floatlabel';
import { Listbox } from 'primeng/listbox';
import { MultiSelect } from 'primeng/multiselect';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { UserAddFromLdapComponent } from '../../../../../../features/bia-features/users-from-directory/views/user-add-from-directory-dialog/user-add-from-directory-dialog.component';
import { Members } from '../../model/member';

@Component({
  selector: 'bia-member-form-new',
  templateUrl: './member-form-new.component.html',
  styleUrls: ['./member-form-new.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MultiSelect,
    NgIf,
    ButtonDirective,
    Listbox,
    UserAddFromLdapComponent,
    TranslateModule,
    FloatLabel,
  ],
})
export class MemberFormNewComponent implements OnChanges {
  @Input() members: Members = <Members>{};
  @Input() roleOptions: OptionDto[];
  @Input() userOptions: OptionDto[];
  @Input() canAddFromDirectory = false;

  @Output() save = new EventEmitter<Members>();
  @Output() cancelled = new EventEmitter<void>();

  form: UntypedFormGroup;
  displayUserAddFromDirectoryDialog = false;

  constructor(
    public formBuilder: UntypedFormBuilder,
    public translateService: TranslateService
  ) {
    this.initForm();
  }

  ngOnChanges() {
    if (this.members) {
      const userSelected = (<Members>this.form.value).users;
      if (userSelected) {
        this.members.users = this.members.users.concat(userSelected);
      }
      this.form.reset();
      this.form.patchValue({ ...this.members });
    }
  }

  protected initForm() {
    this.form = this.formBuilder.group({
      users: [this.members.users, Validators.required],
      roles: [],
    });
  }

  onCancel() {
    this.form.reset();
    this.cancelled.next();
  }

  addUserFromDirectory() {
    this.displayUserAddFromDirectoryDialog = true;
  }

  onSubmit() {
    if (this.form.valid) {
      const members: Members = <Members>this.form.value;
      members.roles = BiaOptionService.differential(
        members.roles,
        this.members?.roles
      );
      members.users = BiaOptionService.differential(members.users, []);

      this.save.emit(members);
      this.form.reset();
    }
  }
}
