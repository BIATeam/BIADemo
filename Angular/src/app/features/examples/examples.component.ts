import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { LocaleDatePipe } from 'packages/bia-ng/shared/public-api';
import { ButtonDirective } from 'primeng/button';
import { Tooltip } from 'primeng/tooltip';
import { AppState } from 'src/app/store/state';
import {
  generateFileDownloadNotification,
  generateHandledError,
  generateUnhandledError,
} from './store/examples-actions';

@Component({
  selector: 'app-examples',
  templateUrl: './examples.component.html',
  styleUrls: ['./examples.component.scss'],
  imports: [ButtonDirective, Tooltip, LocaleDatePipe],
  standalone: true,
})
export class ExamplesComponent {
  date = new Date();

  constructor(private store: Store<AppState>) {}

  onGenerateUnhandledErrorClick() {
    this.store.dispatch(generateUnhandledError());
  }

  onGenerateHandledErrorClick() {
    this.store.dispatch(generateHandledError());
  }

  onGenerateFileDownloadNotificationClick() {
    this.store.dispatch(generateFileDownloadNotification());
  }
}
