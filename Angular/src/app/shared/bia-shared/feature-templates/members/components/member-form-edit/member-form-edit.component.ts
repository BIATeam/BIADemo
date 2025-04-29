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
import { InputText } from 'primeng/inputtext';
import { Listbox } from 'primeng/listbox';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { UserAddFromLdapComponent } from '../../../../../../features/bia-features/users-from-directory/views/user-add-from-directory-dialog/user-add-from-directory-dialog.component';
import { Member } from '../../model/member';

@Component({
  selector: 'bia-member-form-edit',
  templateUrl: './member-form-edit.component.html',
  styleUrls: ['./member-form-edit.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    InputText,
    ButtonDirective,
    Listbox,
    UserAddFromLdapComponent,
    TranslateModule,
    FloatLabel,
  ],
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

  get userData(): string | null {
    return !this.member || Object.keys(this.member).length === 0
      ? null
      : `${this.member?.lastName} ${this.member?.firstName} (${this.member?.login})`;
  }
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
