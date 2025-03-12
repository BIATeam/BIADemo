import { Component } from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { Engine } from '../../model/engine';

@Component({
    selector: 'app-engine-form',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
    styleUrls: [
        '../../../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
    ],
    standalone: false
})
export class EngineFormComponent extends CrudItemFormComponent<Engine> {}
