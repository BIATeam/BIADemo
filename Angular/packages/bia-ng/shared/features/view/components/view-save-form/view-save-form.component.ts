import { CommonModule } from '@angular/common';
import {
  Component,
  Input,
  OnChanges,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { DtoState, ViewType } from 'packages/bia-ng/models/enum/public-api';
import { PermissionTeams, Team } from 'packages/bia-ng/models/public-api';
import { PrimeTemplate } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { Listbox } from 'primeng/listbox';
import { RadioButton } from 'primeng/radiobutton';
import { BiaFormComponent } from '../../../../components/form/bia-form/bia-form.component';
import { CrudItemFormComponent } from '../../../../feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { FormReadOnlyMode } from '../../../../feature-templates/crud-items/model/crud-config';
import { View } from '../../model/view';
import { ViewTeam } from '../../model/view-team';

@Component({
  selector: 'bia-view-save-form',
  templateUrl: './view-save-form.component.html',
  styleUrls: ['./view-save-form.component.scss'],
  imports: [
    PrimeTemplate,
    CommonModule,
    BiaFormComponent,
    ButtonDirective,
    TranslateModule,
    RadioButton,
    ReactiveFormsModule,
    Listbox,
  ],
})
export class ViewSaveFormComponent
  extends CrudItemFormComponent<View>
  implements OnChanges
{
  @ViewChild(BiaFormComponent) biaForm?: BiaFormComponent<View>;
  @Input() canAddTeamView: boolean;
  @Input() canUpdateTeamView: boolean;
  @Input() permissionAssignTeams?: PermissionTeams;
  @Input() canAddUserView: boolean;
  @Input() canUpdateUserView: boolean;
  @Input() teamList: Team[];

  viewTeamList: ViewTeam[] = [];
  otherAffectedTeams: ViewTeam[] | undefined;
  otherAffectedTeamsLabel: string | undefined;

  get isSaveDisabled(): boolean {
    return (
      !this.biaForm?.form?.valid ||
      (!!this.crudItem?.id &&
        this.crudItem?.viewType !==
          this.biaForm?.form?.controls['viewType'].value)
    );
  }
  get isSaveAsDisabled(): boolean {
    return !this.biaForm?.form?.valid;
  }

  viewType: typeof ViewType = ViewType;

  ngOnChanges(changes: SimpleChanges) {
    if (
      changes['teamList'] ||
      changes['crudItem'] ||
      changes['permissionAssignTeams']
    ) {
      this.otherAffectedTeams = this.crudItem?.viewTeams?.filter(
        team =>
          !this.permissionAssignTeams?.isGlobal &&
          !this.permissionAssignTeams?.teamIds.find(id => id === team.id)
      );
      this.otherAffectedTeamsLabel = this.otherAffectedTeams
        ?.map(t => t.teamTitle)
        .join(', ');

      this.viewTeamList = this.teamList
        .filter(
          team =>
            this.permissionAssignTeams?.isGlobal ||
            this.permissionAssignTeams?.teamIds.find(id => id === team.id)
        )
        .map(team => ({
          id: team.id,
          teamTitle: team.title,
          isDefault:
            this.crudItem?.viewTeams?.find(vt => vt.id === team.id)
              ?.isDefault ?? false,
          dtoState: DtoState.Unchanged,
        }));
    }
  }

  saveView(saveAs: boolean = false) {
    if (this.biaForm) {
      if (
        this.formReadOnlyMode === FormReadOnlyMode.clickToEdit &&
        this.biaForm?.readOnly === true
      ) {
        this.biaForm.readOnly = false;
        return;
      }

      if (this.biaForm.form?.valid) {
        this.biaForm.submittingForm = true;
        const element: View = this.biaForm.getElement();
        if (saveAs) {
          element.id = 0;
          switch (element.viewType) {
            case ViewType.User:
              element.viewTeams = [];
              break;
            case ViewType.Team:
              element.viewTeams = element.viewTeams.filter(
                team =>
                  !this.otherAffectedTeams?.find(oat => oat.id === team.id)
              );
              break;
          }
        }
        this.save.emit(element);
        setTimeout(() => {
          if (this.biaForm) {
            this.biaForm.submittingForm = false;
          }
        }, 2000);
      }
    }
  }

  selectCurrentTeam() {
    console.error(
      'selectCurrentTeam',
      this.viewTeamList?.length,
      this.biaForm?.form?.controls['viewTeams'].value
    );
    if (
      this.viewTeamList?.length === 1 &&
      !this.biaForm?.form?.controls['viewTeams'].value?.find(
        (t: ViewTeam) => t.id === this.teamList[0].id
      )
    ) {
      this.biaForm?.form?.controls['viewTeams'].setValue([
        ...this.biaForm?.form?.controls['viewTeams'].value,
        this.viewTeamList[0],
      ]);
    }
  }
}
