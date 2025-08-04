import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { BiaTranslationService } from 'packages/bia-ng/core/public-api';
import { HangfireContainerComponent } from 'packages/bia-ng/shared/public-api';
import { Subscription } from 'rxjs';

@Component({
  selector: 'bia-background-task-read-only',
  templateUrl: './background-task-read-only.component.html',
  styleUrls: ['./background-task-read-only.component.scss'],
  imports: [HangfireContainerComponent],
})
export class BackgroundTaskReadOnlyComponent implements OnInit, OnDestroy {
  @HostBinding('class') classes = 'bia-flex';

  protected sub = new Subscription();
  public url: string;

  constructor(protected biaTranslationService: BiaTranslationService) {}

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.appSettings$.subscribe(appSettings => {
        if (appSettings) {
          this.url = `${appSettings.monitoringUrl}`;
        }
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
}
