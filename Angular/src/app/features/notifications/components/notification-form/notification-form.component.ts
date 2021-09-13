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
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { Notification } from '../../model/notification';

@Component({
  selector: 'app-notification-form',
  templateUrl: './notification-form.component.html',
  styleUrls: ['./notification-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class NotificationFormComponent implements OnInit, OnChanges {
  @Input() notification: Notification = <Notification>{};
  @Input() userOptions: OptionDto[];
  @Input() roleOptions: OptionDto[];
  @Input() notificationTypeOptions: OptionDto[];

  @Output() save = new EventEmitter<Notification>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.initForm();
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.notification) {
      this.form.reset();
      if (this.notification) {
        this.form.patchValue({ ...this.notification });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.notification.id],
      title: [this.notification.title, Validators.required],
      description: [this.notification.description, Validators.required],
      type: [this.notification.type, Validators.required],
      read: [this.notification.read],
      createdDate: [this.notification.createdDate, Validators.required],
      createdBy: [this.notification.createdBy],
      notifiedRoles: [this.notification.notifiedRoles],
      notifiedUsers: [this.notification.notifiedUsers],
      targetJson: [this.notification.description],
    });
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const notification: Notification = <Notification>this.form.value;
      notification.id = notification.id > 0 ? notification.id : 0;
      notification.read = notification.read ? notification.read : false;
      notification.createdBy = BiaOptionService.Clone(notification.createdBy);
      notification.notifiedRoles = BiaOptionService.Differential(notification.notifiedRoles, this.notification?.notifiedRoles);
      notification.notifiedUsers = BiaOptionService.Differential(notification.notifiedUsers, this.notification?.notifiedUsers);
      notification.type = {... notification.type};
      this.save.emit(notification);
      this.form.reset();
    }
  }
}

