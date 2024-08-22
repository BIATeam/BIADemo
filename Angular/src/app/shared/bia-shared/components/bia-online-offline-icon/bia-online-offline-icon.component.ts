import { Component, Injector, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BiaOnlineOfflineService } from 'src/app/core/bia-core/services/bia-online-offline.service';

@Component({
  selector: 'bia-online-offline-icon',
  templateUrl: './bia-online-offline-icon.component.html',
  styleUrls: ['./bia-online-offline-icon.component.scss'],
})
export class BiaOnlineOfflineIconComponent implements OnInit {
  protected biaOnlineOfflineService: BiaOnlineOfflineService;
  serverAvailable$: Observable<boolean>;

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
