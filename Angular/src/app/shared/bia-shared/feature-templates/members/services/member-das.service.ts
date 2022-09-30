import { Injectable, Injector } from '@angular/core';
import { Member } from '../model/member';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';

@Injectable({
  providedIn: 'root'
})
export class MemberDas extends AbstractDas<Member> {
  constructor(injector: Injector) {
    super(injector, 'Members');
  }
}
