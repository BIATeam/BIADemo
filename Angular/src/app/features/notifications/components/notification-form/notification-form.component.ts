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
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { Notification } from '../../model/notification';
import { NotificationTranslation } from '../../model/notification-translation';

@Component({
  selector: 'app-notification-form',
  templateUrl: './notification-form.component.html',
  styleUrls: ['./notification-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class NotificationFormComponent implements OnInit, OnChanges {
  @Input() notification: Notification = <Notification>{};
  @Input() userOptions: OptionDto[];
  @Input() permissionOptions: OptionDto[];
  @Input() notificationTypeOptions: OptionDto[];

  @Output() save = new EventEmitter<Notification>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;
  notificationTranslations: FormArray;

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
        this.notificationTranslations.clear();
        if (this.notification.notificationTranslations) 
          this.notification.notificationTranslations.forEach((notificationTranslation) => { this.notificationTranslations.push(this.createTranslation(notificationTranslation)) });
      }
    }
  }

  private initForm() {
    var group : any = {
      id: [this.notification.id],
      title: [this.notification.title, Validators.required],
      description: [this.notification.description, Validators.required],
      type: [this.notification.type, Validators.required],
      read: [this.notification.read],
      createdDate: [this.notification.createdDate, Validators.required],
      createdBy: [this.notification.createdBy],
      notifiedPermissions: [this.notification.notifiedPermissions],
      notifiedUsers: [this.notification.notifiedUsers],
      jData: [this.notification.jData],
      notificationTranslations: this.formBuilder.array([  ])
    }

    this.form = this.formBuilder.group(group);
    this.notificationTranslations = this.form.get('notificationTranslations') as FormArray;
  }

  createTranslation(notificationTranslation : NotificationTranslation): FormGroup {
    return this.formBuilder.group({
      languageId: notificationTranslation.languageId,
      description: notificationTranslation.description,
      title: notificationTranslation.title
    });
  }

  addItem(): void {
    this.notificationTranslations.push(this.createTranslation({languageId:0, title:'', description:''}));
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
      notification.notifiedPermissions = BiaOptionService.Differential(notification.notifiedPermissions, this.notification?.notifiedPermissions);
      notification.notifiedUsers = BiaOptionService.Differential(notification.notifiedUsers, this.notification?.notifiedUsers);
      notification.type = {... notification.type};
      this.save.emit(notification);
      this.form.reset();
    }
  }
}

