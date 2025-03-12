import { Component } from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { Airport } from '../../model/airport';

@Component({
    selector: 'app-airport-form',
    templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
    styleUrls: [
        '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
    ],
    standalone: false
})
export class AirportFormComponent extends CrudItemFormComponent<Airport> {}
