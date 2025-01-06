import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
} from '@angular/core';
import {
  UntypedFormArray,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { JsonValidator } from 'src/app/shared/bia-shared/validators/json.validator';
import { APP_TANSLATION_IDS_TO_NOT_ADD_MANUALY } from 'src/app/shared/constants';
import {
  Notification,
  NotificationTeam,
  NotificationTranslation,
} from '../../model/notification';

@Component({
  selector: 'bia-notification-form',
  templateUrl: './notification-form.component.html',
  styleUrls: ['./notification-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class NotificationFormComponent implements OnChanges {
  @Input() notification: Notification = <Notification>{};
  @Input() teamOptions: OptionDto[];
  @Input() userOptions: OptionDto[];
  @Input() roleOptions: OptionDto[];
  @Input() notificationTypeOptions: OptionDto[];
  @Input() languageOptions: OptionDto[];

  @Output() save = new EventEmitter<Notification>();
  @Output() cancel = new EventEmitter<void>();

  form: UntypedFormGroup;
  notificationTranslations: UntypedFormArray;
  missingLanguageOptions: OptionDto[];
  public missingTranslation = false;
  protected selectionLanguage = false;

  constructor(public formBuilder: UntypedFormBuilder) {
    this.initForm();
  }

  ngOnChanges() {
    if (this.notification) {
      this.form.reset();
      if (this.notification) {
        this.form.patchValue({ ...this.notification });

        const formArray = new UntypedFormArray([]);
        this.notification.notifiedTeams?.forEach(team => {
          formArray.push(
            this.formBuilder.group({
              team: [team.team, Validators.required],
              roles: [team.roles],
              teamTypeId: [team.teamTypeId],
            })
          );
        });
        this.form.setControl('notifiedTeams', formArray);

        this.notificationTranslations.clear();
        if (this.notification.notificationTranslations) {
          this.notification.notificationTranslations.forEach(
            notificationTranslation => {
              this.notificationTranslations.push(
                this.createTranslation(notificationTranslation)
              );
            }
          );
          this.computeMissingTranslation();
        }
      }
    }
  }

  protected initForm() {
    const group: any = {
      id: [this.notification.id],
      title: [this.notification.title, Validators.required],
      description: [this.notification.description, Validators.required],
      type: [this.notification.type, Validators.required],
      read: [this.notification.read],
      createdDate: [this.notification.createdDate, Validators.required],
      createdBy: [
        /*this.notification.createdBy*/
      ],
      notifiedUsers: [this.notification.notifiedUsers],
      jData: [this.notification.jData, JsonValidator.valid],
      notificationTranslations: this.formBuilder.array([]),
      languageToAdd: [],
      notifiedTeams: this.formBuilder.array([]),
    };

    this.form = this.formBuilder.group(group);
    this.notificationTranslations = this.form.get(
      'notificationTranslations'
    ) as UntypedFormArray;
  }

  /**
   * Returns the FormGroup as a Table Row
   */
  protected createNotifiedTeams(): UntypedFormGroup {
    return this.formBuilder.group({
      team: [null, Validators.required],
      roles: [],
      dtoState: [DtoState.Added],
    });
  }

  get notifiedTeams(): UntypedFormArray {
    return this.form.get('notifiedTeams') as UntypedFormArray;
  }

  addNewRow(): void {
    this.notifiedTeams.push(this.createNotifiedTeams());
  }

  onDeleteRow(rowIndex: number): void {
    this.notifiedTeams.removeAt(rowIndex);
  }

  protected onSelectionLanguage(): void {
    if (this.form.value.languageToAdd?.id > 0) {
      this.selectionLanguage = true;
    } else {
      this.selectionLanguage = true;
    }
  }

  createTranslation(
    notificationTranslation: NotificationTranslation
  ): UntypedFormGroup {
    return this.formBuilder.group({
      id: notificationTranslation.id,
      dtoState: notificationTranslation.dtoState,
      languageId: notificationTranslation.languageId,
      description: [notificationTranslation.description, Validators.required],
      title: [notificationTranslation.title, Validators.required],
    });
  }

  addTranslation(): void {
    const formValue = this.form.value;
    this.notificationTranslations.push(
      this.createTranslation(
        new NotificationTranslation(
          0,
          formValue.languageToAdd.id,
          '',
          '',
          DtoState.Added
        )
      )
    );
    this.computeMissingTranslation();
  }

  addAllTranslation(): void {
    this.missingLanguageOptions.forEach(lo =>
      this.notificationTranslations.push(
        this.createTranslation(
          new NotificationTranslation(0, lo.id, '', '', DtoState.Added)
        )
      )
    );
    this.computeMissingTranslation();
  }

  removeTranslation(index: number): void {
    this.notificationTranslations.removeAt(index);
    this.computeMissingTranslation();
  }

  changeTranslation(index: number): void {
    if (
      this.notificationTranslations.at(index).value.dtoState ===
      DtoState.Unchanged
    ) {
      this.notificationTranslations.at(index).value.dtoState =
        DtoState.Modified;
    }
    this.computeMissingTranslation();
  }

  protected computeMissingTranslation() {
    this.missingLanguageOptions = this.languageOptions.filter(
      lo =>
        !this.notificationTranslations.value.find(
          (nt: { languageId: number }) => nt.languageId === lo.id
        ) && !APP_TANSLATION_IDS_TO_NOT_ADD_MANUALY.find(nta => nta === lo.id)
    );
    if (this.missingLanguageOptions.length > 0) {
      this.missingTranslation = true;
    } else {
      this.missingTranslation = false;
    }
  }

  labelTranslation(id: number): string {
    return this.languageOptions.find(lo => lo.id === id)?.display ?? '';
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
      notification.createdBy = BiaOptionService.clone(notification.createdBy);
      notification.notifiedUsers = BiaOptionService.differential(
        notification.notifiedUsers,
        this.notification?.notifiedUsers
      );
      notification.type = new OptionDto(
        notification.type.id,
        notification.type.display,
        notification.type.dtoState
      );
      notification.notificationTranslations =
        NotificationFormComponent.differentialTranslation(
          notification.notificationTranslations,
          this.notification?.notificationTranslations
        );
      notification.notifiedTeams =
        NotificationFormComponent.differentialNotificationTeam(
          notification.notifiedTeams,
          this.notification?.notifiedTeams
        );
      this.save.emit(notification);
      this.form.reset();
    }
  }

  lang(item: any) {
    console.log(item);
  }

  public static differentialTranslation<T extends BaseDto>(
    newList: T[],
    oldList: T[]
  ) {
    let differential: T[] = [];
    if (oldList && Array.isArray(oldList)) {
      // Delete items
      const toDelete = oldList
        .filter(s => !newList || !newList.map(x => x.id).includes(s.id))
        .map(s => <T>{ ...s, dtoState: DtoState.Deleted });

      if (toDelete) {
        differential = differential.concat(toDelete);
      }
    }

    if (newList && Array.isArray(newList)) {
      const toAddOrUpdate = newList?.filter(
        s => s.dtoState === DtoState.Added || s.dtoState === DtoState.Modified
      );

      if (toAddOrUpdate) {
        differential = differential.concat(toAddOrUpdate);
      }
    }

    return differential;
  }

  public static differentialNotificationTeam(
    newList: NotificationTeam[],
    oldList: NotificationTeam[]
  ) {
    let differential: NotificationTeam[] = [];
    if (oldList && Array.isArray(oldList)) {
      // Delete items
      const toDeleted = oldList
        .filter(
          s => !newList || !newList.map(x => x.team.id).includes(s.team.id)
        )
        .map(s => <NotificationTeam>{ ...s, dtoState: DtoState.Deleted });

      if (toDeleted && toDeleted.length > 0) {
        differential = differential.concat(toDeleted);
      }
    }

    if (newList && Array.isArray(newList)) {
      // Add items
      const toAdded = newList
        .filter(
          s => !oldList || !oldList.map(x => x.team.id).includes(s.team.id)
        )
        .map(s => <NotificationTeam>{ ...s, dtoState: DtoState.Added });

      if (toAdded && toAdded.length > 0) {
        differential = differential.concat(toAdded);
      }
    }
    if (
      newList &&
      Array.isArray(newList) &&
      oldList &&
      Array.isArray(oldList)
    ) {
      // TODO set to modified when role change

      const toCheckModified = newList
        .filter(newNT => oldList.some(oldNT => oldNT.team.id == newNT.team.id))
        .map(
          s =>
            <NotificationTeam>{
              ...s,
              roles: BiaOptionService.differential(
                s.roles,
                oldList.filter(oldNT => oldNT.team.id == s.team.id)[0].roles
              ),
            }
        );

      const toModified = toCheckModified
        .filter(newNT => newNT.roles.length > 0)
        .map(
          s =>
            <NotificationTeam>{
              ...s,
              dtoState: DtoState.Modified,
            }
        );

      if (toModified && toModified.length > 0) {
        differential = differential.concat(toModified);
      }
    }

    return differential;
  }
}
