import { Component, Injector, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { OnlineOfflineService } from 'src/app/core/bia-core/services/online-offline.service';

@Component({
  selector: 'bia-online-offline-icon',
  templateUrl: './online-offline-icon.component.html',
  styleUrls: ['./online-offline-icon.component.scss']
})
export class OnlineOfflineIconComponent implements OnInit {

  public onlineOfflineService: OnlineOfflineService;
  public serverAvailable$: Observable<boolean>;

  constructor(injector: Injector) {
    if (OnlineOfflineService.isModeEnabled) {
      this.onlineOfflineService = injector.get<OnlineOfflineService>(OnlineOfflineService);
    }
  }

  ngOnInit() {
    this.serverAvailable$ = this.onlineOfflineService?.serverAvailable$;
  }
}
