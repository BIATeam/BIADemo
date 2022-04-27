import { Component, HostBinding } from '@angular/core';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';

@Component({
  selector: 'app-background-task-read-only',
  templateUrl: './background-task-read-only.component.html',
  styleUrls: ['./background-task-read-only.component.scss']
})
export class BackgroundTaskReadOnlyComponent {
  @HostBinding('class.bia-flex') flex = true;

  private sub = new Subscription();
  public url : string;

  constructor(private biaTranslationService: BiaTranslationService) {
  }

  ngOnInit(): void {
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
