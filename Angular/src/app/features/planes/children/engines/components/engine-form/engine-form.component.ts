import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import { Component } from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { Engine } from '../../model/engine';
import { BiaSharedModule } from '../../../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-engine-form',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
    styleUrls: [
        '../../../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
    ],
    imports: [BiaSharedModule, BiaFormComponent]
})
export class EngineFormComponent extends CrudItemFormComponent<Engine> {}
