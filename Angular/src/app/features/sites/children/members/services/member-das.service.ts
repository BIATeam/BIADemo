import { Injectable, Injector } from '@angular/core';
import { Member } from '../model/member';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
//import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class MemberDas extends AbstractDas<Member> {
  constructor(injector: Injector/*, private translate: TranslateService*/) {
    super(injector, 'Members');
  }
}
