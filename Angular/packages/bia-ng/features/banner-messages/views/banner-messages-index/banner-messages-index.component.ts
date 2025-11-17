import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from 'packages/bia-ng/core/public-api';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
  SafeHtmlPipe,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { Permission } from 'src/app/shared/permission';
import { bannerMessageCRUDConfiguration } from '../../banner-message.constants';
import { BannerMessage } from '../../model/banner-message';
import { BannerMessageService } from '../../services/banner-message.service';

@Component({
  selector: 'app-banner-messages-index',
  templateUrl: './banner-messages-index.component.html',
  styleUrls: ['./banner-messages-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
    SafeHtmlPipe,
  ],
  providers: [{ provide: CrudItemService, useExisting: BannerMessageService }],
})
export class BannerMessagesIndexComponent
  extends CrudItemsIndexComponent<BannerMessage>
  implements OnInit
{
  // BIAToolKit - Begin BannerMessageIndexTsCanViewChildDeclaration
  // BIAToolKit - End BannerMessageIndexTsCanViewChildDeclaration

  constructor(
    protected injector: Injector,
    public bannerMessageService: BannerMessageService,
    protected authService: AuthService
  ) {
    super(injector, bannerMessageService);
    this.crudConfiguration = bannerMessageCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(
      Permission.BannerMessage_Update
    );
    this.canDelete = this.authService.hasPermission(
      Permission.BannerMessage_Delete
    );
    this.canAdd = this.authService.hasPermission(
      Permission.BannerMessage_Create
    );
    this.canSelect = this.canDelete;
    // BIAToolKit - Begin BannerMessageIndexTsCanViewChildSet
    // BIAToolKit - End BannerMessageIndexTsCanViewChildSet
  }

  // BIAToolKit - Begin BannerMessageIndexTsOnViewChild
  // BIAToolKit - End BannerMessageIndexTsOnViewChild

  protected initSelectedButtonGroup() {
    this.selectionActionsMenuItems = [
      // BIAToolKit - Begin BannerMessageIndexTsSelectedButtonViewChild
      // BIAToolKit - End BannerMessageIndexTsSelectedButtonViewChild
    ];
  }
}
