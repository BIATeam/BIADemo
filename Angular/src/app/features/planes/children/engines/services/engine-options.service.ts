import { Injectable } from '@angular/core';
import { OptionDto } from '@bia-team/bia-ng/models';
import { CrudItemOptionsService, DictOptionDto } from '@bia-team/bia-ng/shared';
import { Store } from '@ngrx/store';
import { Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllPartOptions } from 'src/app/domains/part-option/store/part-option.state';
import { DomainPartOptionsActions } from 'src/app/domains/part-option/store/part-options-actions';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class EngineOptionsService extends CrudItemOptionsService {
  partOptions$: Observable<OptionDto[]>;

  constructor(private store: Store<AppState>) {
    super();
    // TODO after creation of CRUD Engine : get all required option dto use in Table calc and create and edit form
    this.partOptions$ = this.store.select(getAllPartOptions);
    let cpt = 0;
    const part = cpt++;

    this.dictOptionDtos$ = combineLatest([this.partOptions$]).pipe(
      map(options => {
        return <DictOptionDto[]>[
          new DictOptionDto('principalPart', options[part]),
          new DictOptionDto('installedParts', options[part]),
        ];
      })
    );
  }

  loadAllOptions() {
    this.store.dispatch(DomainPartOptionsActions.loadAll());
  }
}
