import { Injectable } from '@angular/core';
import {
  CrudItemOptionsService,
  DictOptionDto,
} from 'packages/bia-ng/shared/public-api';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class BannerMessageOptionsService extends CrudItemOptionsService {
  constructor() {
    super();
    // TODO after creation of CRUD BannerMessage : get all required option dto use in Table calc and create and edit form

    this.dictOptionDtos$ = combineLatest([]).pipe(
      map(() => <DictOptionDto[]>[])
    );
  }
}
