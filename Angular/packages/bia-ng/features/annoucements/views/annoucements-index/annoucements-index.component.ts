import { AsyncPipe, NgClass } from '@angular/common';
import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
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
import { annoucementCRUDConfiguration } from '../../annoucement.constants';
import { Annoucement } from '../../model/annoucement';
import { AnnoucementService } from '../../services/annoucement.service';

@Component({
  selector: 'app-annoucements-index',
  templateUrl: './annoucements-index.component.html',
  styleUrls: ['./annoucements-index.component.scss'],
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
  providers: [{ provide: CrudItemService, useExisting: AnnoucementService }],
})
export class AnnoucementsIndexComponent
  extends CrudItemsIndexComponent<Annoucement>
  implements OnInit, AfterViewInit
{
  // BIAToolKit - Begin AnnoucementIndexTsCanViewChildDeclaration
  // BIAToolKit - End AnnoucementIndexTsCanViewChildDeclaration

  constructor(
    protected injector: Injector,
    public annoucementService: AnnoucementService,
    protected authService: AuthService
  ) {
    super(injector, annoucementService);
    this.crudConfiguration = annoucementCRUDConfiguration;
  }

  ngAfterViewInit(): void {
    this.multiSortMeta = [{ field: 'start', order: -1 }];
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(
      Permission.Annoucement_Update
    );
    this.canDelete = this.authService.hasPermission(
      Permission.Annoucement_Delete
    );
    this.canAdd = this.authService.hasPermission(Permission.Annoucement_Create);
    this.canSelect = this.canDelete;
    // BIAToolKit - Begin AnnoucementIndexTsCanViewChildSet
    // BIAToolKit - End AnnoucementIndexTsCanViewChildSet
  }

  // BIAToolKit - Begin AnnoucementIndexTsOnViewChild
  // BIAToolKit - End AnnoucementIndexTsOnViewChild

  protected initSelectedButtonGroup() {
    this.selectionActionsMenuItems = [
      // BIAToolKit - Begin AnnoucementIndexTsSelectedButtonViewChild
      // BIAToolKit - End AnnoucementIndexTsSelectedButtonViewChild
    ];
  }
}
