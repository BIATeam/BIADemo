import { Injectable } from '@angular/core';
import {
  // BIAToolKit - Begin Option
  Observable,
  // BIAToolKit - End Option
  combineLatest,
} from 'rxjs';
import { map } from 'rxjs/operators';
// BIAToolKit - Begin Option Part
import { getAllPartOptions } from 'src/app/domains/part-option/store/part-option.state';
import { DomainPartOptionsActions } from 'src/app/domains/part-option/store/part-options-actions';
// BIAToolKit - End Option Part
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';
// BIAToolKit - Begin Option
import { Store } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { AppState } from 'src/app/store/state';
// BIAToolKit - End Option

@Injectable({
  providedIn: 'root',
})
export class EngineOptionsService extends CrudItemOptionsService {
  // BIAToolKit - Begin Option Part
  partsOptions$: Observable<OptionDto[]>;
  // BIAToolKit - End Option Part

  constructor(
    // BIAToolKit - Begin Option
    private store: Store<AppState>
    // BIAToolKit - End Option
  ) {
    super();
    // TODO after creation of CRUD Team MaintenanceTeam : get all required option dto use in Table calc and create and edit form
    // BIAToolKit - Begin Option Part
    this.partsOptions$ = this.store.select(getAllPartOptions);
    // BIAToolKit - End Option Part
    // BIAToolKit - Begin Option
    let cpt = 0;
    // BIAToolKit - End Option
    // BIAToolKit - Begin Option Part
    const partType = cpt++;
    // BIAToolKit - End Option Part

    this.dictOptionDtos$ = combineLatest([
      // BIAToolKit - Begin Option Part
      this.partsOptions$,
      // BIAToolKit - End Option Part
    ]).pipe(
      map(
        (
          // BIAToolKit - Begin Option
          options
          // BIAToolKit - End Option
        ) => {
          return <DictOptionDto[]>[
            // BIAToolKit - Begin Option Part
            new DictOptionDto('principalPart', options[partType]),
            new DictOptionDto('installedParts', options[partType]),
            // BIAToolKit - End Option Part
          ];
        }
      )
    );
  }

  // BIAToolKit - Begin Option
  loadAllOptions() {
    // BIAToolKit - End Option
    // BIAToolKit - Begin Option Part
    this.store.dispatch(DomainPartOptionsActions.loadAll());
    // BIAToolKit - End Option Part
    // BIAToolKit - Begin Option
  }
  // BIAToolKit - End Option
}
