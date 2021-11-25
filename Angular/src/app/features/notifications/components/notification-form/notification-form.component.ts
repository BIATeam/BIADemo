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
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { APP_TANSLATION_IDS_TO_NOT_ADD_MANUALY } from 'src/app/shared/constants';
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
  @Input() languageOptions: OptionDto[];

  @Output() save = new EventEmitter<Notification>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;
  notificationTranslations: FormArray;
  missingLanguageOptions: OptionDto[];

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
        {
          this.notification.notificationTranslations.forEach((notificationTranslation) => { this.notificationTranslations.push(this.createTranslation(notificationTranslation)) });
          this.computeMissingTranslation();
        }
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
      notificationTranslations: this.formBuilder.array([  ]),
      languageToAdd : []
    }

    this.form = this.formBuilder.group(group);
    this.notificationTranslations = this.form.get('notificationTranslations') as FormArray;
  }


  protected selectionLanguage: boolean = false;
  protected onSelectionLanguage(): void {
    if (this.form.value.languageToAdd?.id > 0)
    {
      this.selectionLanguage = true;
    }
    else{
      this.selectionLanguage = true;
    }
    
  }

  createTranslation(notificationTranslation : NotificationTranslation): FormGroup {
    return this.formBuilder.group({
      id: notificationTranslation.id,
      DtoState : notificationTranslation.dtoState,
      languageId: notificationTranslation.languageId,
      description: notificationTranslation.description,
      title: notificationTranslation.title
    });
  }

  addTranslation(): void {
    const formValue = this.form.value;
    this.notificationTranslations.push(this.createTranslation({id: 0, languageId:formValue.languageToAdd.id, title:'', description:'', dtoState: DtoState.Added }));
    this.computeMissingTranslation();
  }

  removeTranslation(index : number): void {
    this.notificationTranslations.removeAt(index);
    this.computeMissingTranslation();
  }

  
  private computeMissingTranslation() {
    this.missingLanguageOptions = this.languageOptions.filter(
      lo => !this.notificationTranslations.value.find((nt: { languageId: number; }) => nt.languageId == lo.id)
         && ! APP_TANSLATION_IDS_TO_NOT_ADD_MANUALY.find(nta => nta == lo.id)
    );
  }

  labelTranslation(id : number) : string | undefined {
    return this.languageOptions.find(lo => lo.id == id)?.display;
  }

  
  public static DifferentialTranslation<T extends BaseDto>(newList: T[], oldList: T[]) {
    let differential: T[] = [];
    if (oldList && Array.isArray(oldList)) {
      // Delete items
      const toDelete = oldList
        .filter((s) => !newList || !newList.map(x => x.id).includes(s.id))
        .map((s) => <T>{ ...s, dtoState: DtoState.Deleted });

      if (toDelete) {
        differential = differential.concat(toDelete);
      }
    }

    if (newList) {
      // Add items
      const toAdd = newList
        ?.filter((s) => !oldList || !oldList.map(x => x.id).includes(s.id))
        .map((s) => <T>{ ...s, dtoState: DtoState.Added });

      if (toAdd) {
        differential = differential.concat(toAdd);
      }
    }
    return differential;
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
      notification.notificationTranslations = NotificationFormComponent.DifferentialTranslation(notification.notificationTranslations, this.notification?.notificationTranslations);
      this.save.emit(notification);
      this.form.reset();
    }
  }

  lang(item : any)
  {
    console.log (item);
  }
}

