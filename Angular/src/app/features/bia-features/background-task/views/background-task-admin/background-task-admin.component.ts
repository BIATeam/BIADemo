import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';

@Component({
  selector: 'bia-background-task-admin',
  templateUrl: './background-task-admin.component.html',
  styleUrls: ['./background-task-admin.component.scss']
})
export class BackgroundTaskAdminComponent implements OnInit, OnDestroy {
  @HostBinding('class.bia-flex') flex = true;

  private sub = new Subscription();
  public url : string;

  constructor(private biaTranslationService: BiaTranslationService) {
  }

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
