import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  DomainBannerMessageTypeOptionsActions,
  DomainBannerMessageTypeOptionsStore,
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
export class BannerMessageOptionsService extends CrudItemOptionsService {
  bannerMessageTypeOptions$: Observable<OptionDto[]>;

  constructor(private store: Store<AppState>) {
    super();
    // TODO after creation of CRUD BannerMessage : get all required option dto use in Table calc and create and edit form
    this.bannerMessageTypeOptions$ = this.store.select(
      DomainBannerMessageTypeOptionsStore.getAllBannerMessageTypeOptions
    );

    this.dictOptionDtos$ = combineLatest([this.bannerMessageTypeOptions$]).pipe(
      map(options => <DictOptionDto[]>[new DictOptionDto('type', options[0])])
    );
  }

  loadAllOptions() {
    this.store.dispatch(DomainBannerMessageTypeOptionsActions.loadAll());
  }
}
