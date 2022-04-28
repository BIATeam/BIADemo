import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  @Input() teamId: number;
  @Input() canAddFromDirectory = false;


  @Output() save = new EventEmitter<Members>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;
  displayUserAddFromDirectoryDialog = false;

  constructor(
    public formBuilder: FormBuilder,
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

      // force the parent key => siteId from authService or other Id from 'parent'Service
      members.teamId = this.teamId;
      this.save.emit(members);
      this.form.reset();
    }
  }
}

