import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { Team } from '../model/team';

@Injectable({
  providedIn: 'root'
})
export class TeamDas extends AbstractDas<Team> {
  constructor(injector: Injector) {
    super(injector, 'Teams');
  }
}


















