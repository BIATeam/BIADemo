import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  DomainAnnoucementTypeOptionsActions,
  DomainAnnoucementTypeOptionsStore,
} from 'packages/bia-ng/domains/public-api';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import {
  CrudItemOptionsService,
  DictOptionDto,
} from 'packages/bia-ng/shared/public-api';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AppState } from 'src/app/store/state';

@Injectable({
  providedIn: 'root',
})
export class AnnoucementOptionsService extends CrudItemOptionsService {
  annoucementTypeOptions$: Observable<OptionDto[]>;

  constructor(private store: Store<AppState>) {
    super();
    // TODO after creation of CRUD Annoucement : get all required option dto use in Table calc and create and edit form
    this.annoucementTypeOptions$ = this.store.select(
      DomainAnnoucementTypeOptionsStore.getAllAnnoucementTypeOptions
    );

    this.dictOptionDtos$ = combineLatest([this.annoucementTypeOptions$]).pipe(
      map(options => <DictOptionDto[]>[new DictOptionDto('type', options[0])])
    );
  }

  loadAllOptions() {
    this.store.dispatch(DomainAnnoucementTypeOptionsActions.loadAll());
  }
}
