import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { Member } from '../model/member';

@Injectable({
  providedIn: 'root',
})
export class MemberDas extends AbstractDas<Member> {
  constructor(injector: Injector) {
    super(injector, 'Members');
  }
}
