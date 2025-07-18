import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'biang/core';
import { Member } from '../model/member';

@Injectable({
  providedIn: 'root',
})
export class MemberDas extends AbstractDas<Member> {
  constructor(injector: Injector) {
    super(injector, 'Members');
  }
}
