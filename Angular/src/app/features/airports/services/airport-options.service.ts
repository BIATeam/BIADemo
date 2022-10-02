import { Injectable } from '@angular/core';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';
import { CrudItemOptionsService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-options.service';

@Injectable({
    providedIn: 'root'
})
export class AirportOptionsService extends CrudItemOptionsService {

    constructor(
    ) {
        super();
        // TODO after creation of CRUD Airport : get all requiered option dto use in Table calc and create and edit form

        this.dictOptionDtos$ = combineLatest([]).pipe(
            map(
                (options) =>
                <DictOptionDto[]>[
                ]
            )
        );
    }

    loadAllOptions() {
    }
}
