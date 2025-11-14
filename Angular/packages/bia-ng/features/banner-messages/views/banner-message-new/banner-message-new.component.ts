import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemNewComponent } from 'packages/bia-ng/shared/public-api';
import { bannerMessageCRUDConfiguration } from '../../banner-message.constants';
import { BannerMessageFormComponent } from '../../components/banner-message-form/banner-message-form.component';
import { BannerMessage } from '../../model/banner-message';
import { BannerMessageService } from '../../services/banner-message.service';

@Component({
  selector: 'app-banner-message-new',
  templateUrl: './banner-message-new.component.html',
  imports: [BannerMessageFormComponent, AsyncPipe],
})
export class BannerMessageNewComponent
  extends CrudItemNewComponent<BannerMessage>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    public bannerMessageService: BannerMessageService
  ) {
    super(injector, bannerMessageService);
    this.crudConfiguration = bannerMessageCRUDConfiguration;
  }
}
