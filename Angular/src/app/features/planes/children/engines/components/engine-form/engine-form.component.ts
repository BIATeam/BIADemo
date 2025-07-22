import { Component } from '@angular/core';
import { BiaFormComponent, CrudItemFormComponent } from 'biang/shared';
import { Engine } from '../../model/engine';

@Component({
  selector: 'app-engine-form',
  templateUrl:
    '../../../../../../../../node_modules/biang/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../../../node_modules/biang/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class EngineFormComponent extends CrudItemFormComponent<Engine> {}
