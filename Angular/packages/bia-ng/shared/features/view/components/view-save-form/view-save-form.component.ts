import { CommonModule } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { ButtonDirective } from 'primeng/button';
import { BiaFormComponent } from '../../../../components/form/bia-form/bia-form.component';
import { CrudItemFormComponent } from '../../../../feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { FormReadOnlyMode } from '../../../../feature-templates/crud-items/model/crud-config';
import { View } from '../../model/view';

@Component({
  selector: 'bia-view-save-form',
  templateUrl: './view-save-form.component.html',
  styleUrls: ['./view-save-form.component.scss'],
  imports: [CommonModule, BiaFormComponent, ButtonDirective, TranslateModule],
})
export class ViewSaveFormComponent extends CrudItemFormComponent<View> {
  @ViewChild(BiaFormComponent) biaForm: BiaFormComponent<View>;
  isSaveDisabled: any;
  isSaveAsDisabled: any;

  saveView(saveAs: boolean = false) {
    if (
      this.formReadOnlyMode === FormReadOnlyMode.clickToEdit &&
      this.biaForm.readOnly === true
    ) {
      this.biaForm.readOnly = false;
      return;
    }

    if (this.biaForm.form?.valid) {
      this.biaForm.submittingForm = true;
      const element: any = this.biaForm.getElement();
      if (saveAs) {
        element.id = 0;
        //TODO save only on current team
        element.viewTeams = this.crudItem?.viewTeams;
      } else {
        element.viewTeams = this.crudItem?.viewTeams;
      }
      this.save.emit(element);
      setTimeout(() => {
        this.biaForm.submittingForm = false;
      }, 2000);
    }
  }
}
