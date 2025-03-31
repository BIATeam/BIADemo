import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { HangfireContainerComponent } from 'src/app/shared/bia-shared/components/hangfire-container/hangfire-container.component';

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
