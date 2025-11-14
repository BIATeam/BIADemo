import { AfterViewInit, Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { BiaBannerType } from 'packages/bia-ng/models/enum/bia-banner-type.enum';
import { DtoState } from 'packages/bia-ng/models/enum/dto-state.enum';
import {
  BiaFormComponent,
  CrudItemFormComponent,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { EditorModule } from 'primeng/editor';
import { FloatLabel } from 'primeng/floatlabel';
import { Select } from 'primeng/select';
import { BannerMessage } from '../../model/banner-message';

@Component({
  selector: 'app-banner-message-form',
  templateUrl: 'banner-message-form.component.html',
  styleUrls: ['banner-message-form.component.scss'],
  imports: [
    BiaFormComponent,
    Select,
    EditorModule,
    FloatLabel,
    TranslatePipe,
    PrimeTemplate,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class BannerMessageFormComponent
  extends CrudItemFormComponent<BannerMessage>
  implements AfterViewInit
{
  ngAfterViewInit(): void {
    this.dictOptionDtos = [
      {
        key: 'type',
        value: [
          {
            id: BiaBannerType.Info,
            display: 'bia.info',
            dtoState: DtoState.Unchanged,
          },
          {
            id: BiaBannerType.Warning,
            display: 'bia.warning',
            dtoState: DtoState.Unchanged,
          },
        ],
      },
    ];
  }
}
