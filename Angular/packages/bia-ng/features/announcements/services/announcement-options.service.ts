import { Injectable } from '@angular/core';
import {
  DomainAnnouncementTypeOptionsActions,
  DomainAnnouncementTypeOptionsStore,
} from '@bia-team/bia-ng/domains';
import { OptionDto } from '@bia-team/bia-ng/models';
import { CrudItemOptionsService, DictOptionDto } from '@bia-team/bia-ng/shared';
import { BiaAppState } from '@bia-team/bia-ng/store';
import { Store } from '@ngrx/store';
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
