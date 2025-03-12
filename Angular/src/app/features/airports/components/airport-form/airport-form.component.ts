import { Component } from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { Airport } from '../../model/airport';
import { BiaFormComponent } from '../../../../shared/bia-shared/components/form/bia-form/bia-form.component';

@Component({
    selector: 'app-airport-form',
    templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
    styleUrls: [
        '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
    ],
    imports: [BiaFormComponent]
})
export class AirportFormComponent extends CrudItemFormComponent<Airport> {}
