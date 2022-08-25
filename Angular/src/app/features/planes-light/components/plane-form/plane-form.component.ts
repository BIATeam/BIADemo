import {
  ChangeDetectionStrategy,
  Component,
} from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { Plane } from '../../model/plane';

@Component({
  selector: 'app-plane-form',
  templateUrl: './plane-form.component.html',
  styleUrls: ['./plane-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class PlaneFormComponent extends CrudItemFormComponent<Plane> {
  constructor(public formBuilder: FormBuilder) {
    super(formBuilder);
  }
}

