import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';

@Injectable({
    providedIn: 'root'
})
export class AircraftMaintenanceCompanyOptionsService {
    dictOptionDtos$: Observable<DictOptionDto[]>;

    constructor() {
    }

    loadAllOptions() {
    }
}
