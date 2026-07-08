import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import {
  BiaLayoutService,
  CrudItemItemComponent,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { first } from 'rxjs/operators';
import { AppState } from 'src/app/store/state';
import { Airport } from '../../model/airport';
import { AirportService } from '../../services/airport.service';

@Component({
  selector: 'app-airports-item',
  templateUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
})
export class AirportItemComponent
  extends CrudItemItemComponent<Airport>
  implements OnInit
{
  constructor(
    protected store: Store<AppState>,
    protected injector: Injector,
    public airportService: AirportService,
    protected layoutService: BiaLayoutService
  ) {
    super(injector, airportService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add(
      this.airportService.crudItem$.subscribe(airport => {
        // TODO after creation of CRUD Airport : set the field of the item to display in the breadcrump
        if (airport?.name) {
          this.route.data.pipe(first()).subscribe(routeData => {
            (routeData as any)['breadcrumb'] = airport.name;
          });
          this.layoutService.refreshBreadcrumb();
        }
      })
    );
  }
}
