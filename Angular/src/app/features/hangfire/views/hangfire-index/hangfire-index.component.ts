import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, HostBinding } from '@angular/core';
import { Store } from '@ngrx/store';
import { BiaTeamsStore } from 'bia-ng/core';
import { Team } from 'bia-ng/models';
import { ButtonDirective } from 'primeng/button';
import { Tooltip } from 'primeng/tooltip';
import { Observable } from 'rxjs';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { randomReviewPlane } from '../../store/hangfire-actions';

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
    this.allTeams$ = this.store.select(BiaTeamsStore.getAllTeams);
  }

  randomReviewPlane(teamId: number) {
    this.store.dispatch(randomReviewPlane({ teamId }));
  }
}
