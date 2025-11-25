import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { BiaFormComponent, CrudItemFormComponent } from 'bia-ng/shared';
import { View } from '../../model/view';

@Component({
  selector: 'bia-view-save-form',
  templateUrl: './view-save-form.component.html',
  styleUrls: ['./view-save-form.component.scss'],
  imports: [CommonModule, BiaFormComponent],
})
export class ViewSaveFormComponent extends CrudItemFormComponent<View> {}
