import { Component } from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { Site } from '../../model/site';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-site-form',
    templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
    styleUrls: [
        '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
    ],
    imports: [BiaSharedModule]
})
export class SiteFormComponent extends CrudItemFormComponent<Site> {}
