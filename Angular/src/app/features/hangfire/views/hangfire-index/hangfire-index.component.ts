import { Component, HostBinding } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Team } from 'src/app/domains/bia-domains/team/model/team';
import { getAllTeams } from 'src/app/domains/bia-domains/team/store/team.state';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { randomReviewPlane } from '../../store/hangfire-actions';
import { NgFor, NgIf, AsyncPipe } from '@angular/common';
import { ButtonDirective } from 'primeng/button';
import { Tooltip } from 'primeng/tooltip';

@Component({
  selector: 'app-hangfire-index',
  templateUrl: './hangfire-index.component.html',
  styleUrls: ['./hangfire-index.component.scss'],
  imports: [NgFor, NgIf, ButtonDirective, Tooltip, AsyncPipe],
})
export class HangfireIndexComponent {
  @HostBinding('class') classes = 'bia-flex';
  allTeams$: Observable<Team[]>;
  teamTypeId: typeof TeamTypeId = TeamTypeId;

  constructor(private store: Store<AppState>) {
    this.allTeams$ = this.store.select(getAllTeams);
  }

  randomReviewPlane(teamId: number) {
    this.store.dispatch(randomReviewPlane({ teamId }));
  }
}
