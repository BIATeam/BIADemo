import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  DomainAnnouncementTypeOptionsActions,
  DomainAnnouncementTypeOptionsStore,
} from 'packages/bia-ng/domains/public-api';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import {
  CrudItemOptionsService,
  DictOptionDto,
} from 'packages/bia-ng/shared/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementOptionsService extends CrudItemOptionsService {
  announcementTypeOptions$: Observable<OptionDto[]>;

  constructor(private store: Store<BiaAppState>) {
    super();
    // TODO after creation of CRUD Announcement : get all required option dto use in Table calc and create and edit form
    this.announcementTypeOptions$ = this.store.select(
      DomainAnnouncementTypeOptionsStore.getAllAnnouncementTypeOptions
    );

    this.dictOptionDtos$ = combineLatest([this.announcementTypeOptions$]).pipe(
      map(options => <DictOptionDto[]>[new DictOptionDto('type', options[0])])
    );
  }

  loadAllOptions() {
    this.store.dispatch(DomainAnnouncementTypeOptionsActions.loadAll());
  }
}
