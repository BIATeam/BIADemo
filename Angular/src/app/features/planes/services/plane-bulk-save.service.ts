import { Injectable } from '@angular/core';
import { CrudItemBulkSaveService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-bulk-save.service';
import { Plane } from '../model/plane';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { PlaneService } from './plane.service';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { TeamTypeId } from 'src/app/shared/constants';
import { TranslateService } from '@ngx-translate/core';

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
  constructor(
    planeService: PlaneService,
    translateService: TranslateService,
    private authService: AuthService
  ) {
    super(planeService, translateService);
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

  override customMapCsvToJson(plane: Plane, csvPlane: CsvPlane): string[] {
    const errorMessages: string[] = [];

    plane.planeType =
      this.planeTypeOptions.find(
        x => x.display === csvPlane?.planeTypeDisplay?.trim()
      ) ?? null;

    plane.siteId =
      plane.siteId > 0
        ? plane.siteId
        : this.authService.getCurrentTeamId(TeamTypeId.Site);

    if (
      (csvPlane?.planeTypeDisplay?.length > 0 && plane.planeType?.id > 0) !==
      true
    ) {
      errorMessages.push('This type of plane does not exist.');
    }

    return errorMessages;
  }
}
