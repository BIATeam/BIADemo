import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  CrudItemItemComponent,
  CrudItemService,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { BannerMessage } from '../../model/banner-message';
import { BannerMessageService } from '../../services/banner-message.service';

@Component({
  selector: 'app-banner-messages-item',
  templateUrl:
    '../../../../shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
  providers: [
    {
      provide: CrudItemService,
      useExisting: BannerMessageService,
    },
  ],
})
export class BannerMessageItemComponent extends CrudItemItemComponent<BannerMessage> {}
