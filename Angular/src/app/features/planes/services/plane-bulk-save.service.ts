import { Injectable } from '@angular/core';
import { CrudItemBulkSaveService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-bulk-save.service';
import { Plane } from '../model/plane';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { PlaneService } from './plane.service';

export interface CsvPlane extends Plane {
  planeTypeDisplay: string;
}

@Injectable({
  providedIn: 'root',
})
export class PlaneBulkSaveService extends CrudItemBulkSaveService<
  Plane,
  CsvPlane
> {
  constructor(planeService: PlaneService) {
    super(planeService);
  }
  public planeTypeOptions: OptionDto[];

  override customMapJsonToCsv(planes: Plane[]): CsvPlane[] {
    return planes.map(plane => {
      return <CsvPlane>{
        planeTypeDisplay: plane.planeType?.display,
        ...plane,
      };
    });
  }

  override customMapCsvToJson(plane: Plane, csvPlane: CsvPlane) {
    plane.planeType =
      this.planeTypeOptions.find(
        x => x.display === csvPlane?.planeTypeDisplay?.trim()
      ) ?? null;
  }
}
