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
import { TranslateService } from '@ngx-translate/core';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { Member } from '../../model/member';

@Component({
  selector: 'bia-member-form-edit',
  templateUrl: './member-form-edit.component.html',
  styleUrls: ['./member-form-edit.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class MemberFormEditComponent implements OnChanges {
  @Input() member: Member = <Member>{};
  @Input() roleOptions: OptionDto[];
  @Input() userOptions: OptionDto[];
  @Input() canAddFromDirectory = false;

  @Output() save = new EventEmitter<Member>();
  @Output() cancel = new EventEmitter<void>();

  form: UntypedFormGroup;
  displayUserAddFromDirectoryDialog = false;

  constructor(
    public formBuilder: UntypedFormBuilder,
    public translateService: TranslateService) {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.member) {
      this.form.reset();
      if (this.member) {
        this.form.patchValue({ ...this.member });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.member.id],
      user: [this.member.user, Validators.required],
      roles: [this.member.roles],
      teamId: [this.member.teamId],
    });
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  addUserFromDirectory() {
    this.displayUserAddFromDirectoryDialog = true;
  }

  onSubmit() {
    if (this.form.valid) {
      const member: Member = <Member>this.form.value;
      member.id = member.id > 0 ? member.id : 0;
      member.roles = BiaOptionService.Differential(member.roles, this.member?.roles);
      member.user = {...member.user};

      this.save.emit(member);
      this.form.reset();
    }
  }
}

