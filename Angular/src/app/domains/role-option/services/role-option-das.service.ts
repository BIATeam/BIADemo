import { Injectable, Injector } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

@Injectable({
  providedIn: 'root'
})
export class RoleOptionDas extends AbstractDas<OptionDto> {
  constructor(injector: Injector, private translate: TranslateService) {
    super(injector, 'Roles');
  }
  translateItem(item: OptionDto) {
    item.display = this.translate.instant(`role.${item.display.toLowerCase()}`);
    return item;
  }
}


















