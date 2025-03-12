import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
} from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { Member } from '../../model/member';
import { Select } from 'primeng/select';
import { NgIf } from '@angular/common';
import { ButtonDirective } from 'primeng/button';
import { Listbox } from 'primeng/listbox';
import { UserAddFromLdapComponent } from '../../../../../../features/bia-features/users-from-directory/views/user-add-from-directory-dialog/user-add-from-directory-dialog.component';

@Component({
    selector: 'bia-member-form-edit',
    templateUrl: './member-form-edit.component.html',
    styleUrls: ['./member-form-edit.component.scss'],
    changeDetection: ChangeDetectionStrategy.Default,
    imports: [FormsModule, ReactiveFormsModule, Select, NgIf, ButtonDirective, Listbox, UserAddFromLdapComponent, TranslateModule]
})
export class MemberFormEditComponent implements OnChanges {
  @Input() member: Member | null = null;
  @Input() roleOptions: OptionDto[];
  @Input() userOptions: OptionDto[];
  @Input() canAddFromDirectory = false;

  @Output() save = new EventEmitter<Member>();
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
    if (this.member) {
      this.form.reset();
      if (this.member) {
        this.form.patchValue({ ...this.member });
      }
    }
  }

  protected initForm() {
    this.form = this.formBuilder.group({
      id: [this.member?.id],
      user: [this.member?.user, Validators.required],
      roles: [this.member?.roles],
      teamId: [this.member?.teamId],
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
      const member: Member = <Member>this.form.value;
      member.id = member.id > 0 ? member.id : 0;
      member.roles = BiaOptionService.differential(
        member.roles,
        this.member?.roles ?? []
      );
      member.user = new OptionDto(
        member.user.id,
        member.user.display,
        member.user.dtoState
      );

      this.save.emit(member);
      this.form.reset();
    }
  }
}
