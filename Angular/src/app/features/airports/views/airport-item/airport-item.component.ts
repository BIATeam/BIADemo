import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { getCurrentAirport} from '../../store/airport.state';
import { Airport } from '../../model/airport';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute } from '@angular/router';
import { AirportService } from '../../services/airport.service';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';

@Component({
  templateUrl: './airport-item.component.html',
  styleUrls: ['./airport-item.component.scss']
})
export class AirportItemComponent implements OnInit, OnDestroy {
  airport$: Observable<Airport>;
  private sub = new Subscription();

  constructor(private store: Store<AppState>,
    private route: ActivatedRoute,
    public airportService: AirportService,
    private layoutService: BiaClassicLayoutService) { }

  ngOnInit() {
    this.airportService.currentAirportId = this.route.snapshot.params.airportId;
    this.sub.add
      (
        this.store.select(getCurrentAirport).subscribe((airport) => {
          if (airport?.name) {
            this.route.data.pipe(first()).subscribe(routeData => {
              (routeData as any)['breadcrumb'] = airport.name;
            });
            this.layoutService.refreshBreadcrumb();
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
