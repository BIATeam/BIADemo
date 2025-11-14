import { Component } from '@angular/core';
import {
  BiaFormComponent,
  CrudItemFormComponent,
} from 'packages/bia-ng/shared/public-api';
import { BannerMessage } from '../../model/banner-message';

@Component({
  selector: 'app-banner-message-form',
  templateUrl:
    '../../../../shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class BannerMessageFormComponent extends CrudItemFormComponent<BannerMessage> {}
