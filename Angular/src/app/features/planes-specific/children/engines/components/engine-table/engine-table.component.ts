import { Component } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { Engine } from 'src/app/features/planes-specific/model/engine';
import { CrudItemTableComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-table/crud-item-table.component';

@Component({
  selector: 'app-engine-specific-table',
  templateUrl:
    '/src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '/src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss',
  ],
})
export class EngineTableComponent extends CrudItemTableComponent<Engine> {
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }
}
