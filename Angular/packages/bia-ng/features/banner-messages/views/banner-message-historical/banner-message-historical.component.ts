import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import {
  CrudItemHistoricalComponent,
  CrudItemHistoricalTimelineComponent,
} from 'packages/bia-ng/shared/public-api';
import { Button } from 'primeng/button';
import { bannerMessageCRUDConfiguration } from '../../banner-message.constants';
import { BannerMessage } from '../../model/banner-message';
import { BannerMessageService } from '../../services/banner-message.service';

@Component({
  selector: 'app-banner-message-historical',
  imports: [
    CrudItemHistoricalTimelineComponent,
    AsyncPipe,
    TranslateModule,
    Button,
  ],
  templateUrl:
    '../../../../shared/feature-templates/crud-items/views/crud-item-historical/crud-item-historical.component.html',
  styleUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-historical/crud-item-historical.component.scss',
})
export class BannerMessageHistoricalComponent extends CrudItemHistoricalComponent<BannerMessage> {
  constructor(
    protected injector: Injector,
    protected bannerMessageService: BannerMessageService
  ) {
    super(injector, bannerMessageService, bannerMessageCRUDConfiguration);
  }
}
