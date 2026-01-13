import { AsyncPipe } from '@angular/common';
import { Component, HostBinding } from '@angular/core';
import { CoreTeamsStore } from '@bia-team/bia-ng/core';
import { Team } from '@bia-team/bia-ng/models';
import { LocaleDatePipe } from '@bia-team/bia-ng/shared';
import { Store } from '@ngrx/store';
import { ButtonDirective } from 'primeng/button';
import { Tooltip } from 'primeng/tooltip';
import { Observable } from 'rxjs';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import {
  generateHandledError,
  generateUnhandledError,
  randomReviewPlane,
} from '../../store/hangfire-actions';

@Component({
  selector: 'app-hangfire-index',
  templateUrl: './hangfire-index.component.html',
  styleUrls: ['./hangfire-index.component.scss'],
  imports: [ButtonDirective, Tooltip, AsyncPipe, LocaleDatePipe],
})
export class HangfireIndexComponent {
  @HostBinding('class') classes = 'bia-flex';
  allTeams$: Observable<Team[]>;
  teamTypeId: typeof TeamTypeId = TeamTypeId;
  date: Date = new Date(Date.now());

  constructor(private store: Store<AppState>) {
    this.allTeams$ = this.store.select(CoreTeamsStore.getAllTeams);
  }

  randomReviewPlane(teamId: number) {
    this.store.dispatch(randomReviewPlane({ teamId }));
  }

  onGenerateUnhandledErrorClick() {
    this.store.dispatch(generateUnhandledError());
  }

  onGenerateHandledErrorClick() {
    this.store.dispatch(generateHandledError());
  }
}
