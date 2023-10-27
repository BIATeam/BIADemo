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
import { Members } from '../../model/member';

@Component({
  selector: 'bia-member-form-new',
  templateUrl: './member-form-new.component.html',
  styleUrls: ['./member-form-new.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class MemberFormNewComponent implements OnChanges {
  @Input() members: Members = <Members>{};
  @Input() roleOptions: OptionDto[];
  @Input() userOptions: OptionDto[];
  @Input() canAddFromDirectory = false;


  @Output() save = new EventEmitter<Members>();
  @Output() cancel = new EventEmitter<void>();

  form: UntypedFormGroup;
  displayUserAddFromDirectoryDialog = false;

  constructor(
    public formBuilder: UntypedFormBuilder,
    public translateService: TranslateService) {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.members) {
      let userSelected = (<Members>this.form.value).users;
      if (userSelected)
      {
        this.members.users = this.members.users.concat(userSelected);
      }
      this.form.reset();
      this.form.patchValue({ ...this.members });
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      users: [this.members.users, Validators.required],
      roles: [],
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
      const members: Members = <Members>this.form.value;
      members.roles = BiaOptionService.Differential(members.roles, this.members?.roles);
      members.users = BiaOptionService.Differential(members.users, []);

      this.save.emit(members);
      this.form.reset();
    }
  }
}

