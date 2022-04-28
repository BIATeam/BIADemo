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
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { User } from '../../model/user';

@Component({
  selector: 'bia-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class UserFormComponent implements OnChanges {
  @Input() user: User = <User>{};
  @Input() roleOptions: OptionDto[];

  @Output() save = new EventEmitter<User>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.user) {
      this.form.reset();
      if (this.user) {
        this.form.patchValue({ ...this.user });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.user.id],
      firstName: [this.user.firstName, Validators.required],
      lastName: [this.user.lastName, Validators.required],
      login: [this.user.login, Validators.required],
      roles: [this.user.roles],
    });

  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const user: User = <User>this.form.value;
      user.id = user.id > 0 ? user.id : 0;
      user.roles = BiaOptionService.Differential(user.roles, this.user?.roles);

      this.save.emit(user);
      this.form.reset();
    }
  }
}

