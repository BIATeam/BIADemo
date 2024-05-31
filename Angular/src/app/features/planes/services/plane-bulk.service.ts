import { Injectable } from '@angular/core';
import { CrudItemBulkService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-bulk.service';
import { Plane } from '../model/plane';
import { PlaneService } from './plane.service';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class PlaneBulkService extends CrudItemBulkService<Plane> {
  constructor(planeService: PlaneService, translateService: TranslateService) {
    super(planeService, translateService);
  }
}
