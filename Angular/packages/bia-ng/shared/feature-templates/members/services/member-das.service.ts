import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Member } from '../model/member';

@Injectable({
  providedIn: 'root',
})
export class MemberDas extends AbstractDas<Member> {
  constructor(injector: Injector) {
    super(injector, 'Members');
  }
}
