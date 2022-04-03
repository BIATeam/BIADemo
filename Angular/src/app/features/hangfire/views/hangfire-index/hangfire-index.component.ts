import { Component, HostBinding } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Team } from 'src/app/domains/team/model/team';
import { getAllTeams } from 'src/app/domains/team/store/team.state';
import { AppState } from 'src/app/store/state';
import { callWorkerWithNotification } from '../../store/hangfire-actions';

@Component({
  selector: 'app-hangfire-index',
  templateUrl: './hangfire-index.component.html',
  styleUrls: ['./hangfire-index.component.scss']
})
export class HangfireIndexComponent {
  @HostBinding('class.bia-flex') flex = true;
  allTeams$: Observable<Team[]>;

  constructor(private store: Store<AppState>) {
    this.allTeams$ = this.store.select(getAllTeams);
  }

  callWorkerWithNotification(teamId: number) {
    this.store.dispatch(callWorkerWithNotification({ teamId }));
  }
}
