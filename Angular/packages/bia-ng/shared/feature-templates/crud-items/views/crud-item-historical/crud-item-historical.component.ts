import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { BiaFieldConfig } from 'packages/bia-ng/models/bia-field-config';
import { BaseDto } from 'packages/bia-ng/models/public-api';
import { Button } from 'primeng/button';
import { CrudItemHistoricalTimelineComponent } from '../../components/crud-item-historical-timeline/crud-item-historical-timeline.component';
import { CrudConfig, CrudItemComponent } from '../../public-api';
import { CrudItemService } from '../../services/crud-item.service';

@Component({
  selector: 'bia-crud-item-historical',
  imports: [
    CrudItemHistoricalTimelineComponent,
    AsyncPipe,
    TranslateModule,
    Button,
  ],
  templateUrl: './crud-item-historical.component.html',
  styleUrl: './crud-item-historical.component.scss',
})
export class CrudItemHistoricalComponent<
  CrudItem extends BaseDto<string | number>,
> extends CrudItemComponent<CrudItem> {
  onSubmitted(crudItemToUpdate: CrudItem): void {
    throw new Error('Method not implemented.');
  }

  protected navigateBack(): void {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  fields: BiaFieldConfig<CrudItem>[];

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemService<CrudItem>,
    public crudConfiguration: CrudConfig<CrudItem>
  ) {
    super(injector, crudItemService);
  }
}
