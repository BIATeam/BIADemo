import { Injectable, Injector } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { HttpOptions } from 'src/app/core/bia-core/services/generic-das.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

@Injectable({
  providedIn: 'root'
})
export class NotificationTypeOptionDas extends AbstractDas<OptionDto> {
  constructor(injector: Injector, private translate: TranslateService) {
    super(injector, 'NotificationTypes');
  }
  getList(endpoint: string = '', options?: HttpOptions): Observable<OptionDto[]> {
    return this.getListItems<OptionDto>(endpoint, options).pipe(map(optionDtos => {
        optionDtos.map(optionDto => {
        optionDto.display = this.translate.instant(`notification.type.${optionDto.display.toLowerCase()}`);
        return optionDto;
      });
      return optionDtos;
    }));
  }
}


















