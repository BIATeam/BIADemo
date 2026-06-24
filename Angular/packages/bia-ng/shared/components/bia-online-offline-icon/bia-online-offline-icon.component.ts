import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { BiaOnlineOfflineService } from '@bia-team/bia-ng/core';
import { TranslateModule } from '@ngx-translate/core';
import { Tooltip } from 'primeng/tooltip';
import { Observable } from 'rxjs';

@Component({
  selector: 'bia-online-offline-icon',
  templateUrl: './bia-online-offline-icon.component.html',
  styleUrls: ['./bia-online-offline-icon.component.scss'],
  imports: [Tooltip, AsyncPipe, TranslateModule],
})
export class BiaOnlineOfflineIconComponent implements OnInit {
  public biaOnlineOfflineService: BiaOnlineOfflineService;
  public serverAvailable$: Observable<boolean>;

  constructor(injector: Injector) {
    if (BiaOnlineOfflineService.isModeEnabled) {
      this.biaOnlineOfflineService = injector.get<BiaOnlineOfflineService>(
        BiaOnlineOfflineService
      );
    }
  }

  ngOnInit() {
    this.serverAvailable$ = this.biaOnlineOfflineService?.serverAvailable$;
  }
}
