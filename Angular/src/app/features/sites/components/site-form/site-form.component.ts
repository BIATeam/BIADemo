import {
  Component,
} from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { Site } from '../../model/site';

@Component({
  selector: 'app-site-form',
  templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: ['../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss'],
})

export class SiteFormComponent extends CrudItemFormComponent<Site> {
}

