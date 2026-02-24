import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { BiaTranslationService } from '@bia-team/bia-ng/core';
import { HangfireContainerComponent } from '@bia-team/bia-ng/shared';
import { Subscription } from 'rxjs';

@Component({
  selector: 'bia-background-task-admin',
  templateUrl: './background-task-admin.component.html',
  styleUrls: ['./background-task-admin.component.scss'],
  imports: [HangfireContainerComponent],
})
export class BackgroundTaskAdminComponent implements OnInit, OnDestroy {
  @HostBinding('class') classes = 'bia-flex';

  protected sub = new Subscription();
  public url: string;

  constructor(protected biaTranslationService: BiaTranslationService) {}

  ngOnInit(): void {
    this.sub.add(
      this.biaTranslationService.appSettings$.subscribe(appSettings => {
        if (appSettings) {
          this.url = `${appSettings.monitoringUrl}Admin`;
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
