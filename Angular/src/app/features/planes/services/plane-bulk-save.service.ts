import { Injectable } from '@angular/core';
import { CrudItemBulkSaveService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-bulk-save.service';
import { Plane } from '../model/plane';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { PlaneService } from './plane.service';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class PlaneBulkSaveService extends CrudItemBulkSaveService<Plane> {
  constructor(planeService: PlaneService, translateService: TranslateService) {
    super(planeService, translateService);
  }
  public planeTypeOptions: OptionDto[];

  override customMapJsonToCsv(planes: Plane[]): Plane[] {
    return planes.map(plane => {
      return <Plane>{
        planeTypeDisplay: plane.planeType?.display,
        ...plane,
      };
    });
  }

  // override customMapCsvToJson(plane: Plane, plane: Plane): string[] {
  //   const errorMessages: string[] = [];

  //   plane.planeType =
  //     this.planeTypeOptions.find(
  //       x => x.display === plane?.planeTypeDisplay?.trim()
  //     ) ?? null;

  //   plane.siteId =
  //     plane.siteId > 0
  //       ? plane.siteId
  //       : this.authService.getCurrentTeamId(TeamTypeId.Site);

  //   if (
  //     (plane?.planeTypeDisplay?.length > 0 && plane.planeType?.id > 0) !==
  //     true
  //   ) {
  //     errorMessages.push('This type of plane does not exist.');
  //   }

  //   return errorMessages;
  // }
}
