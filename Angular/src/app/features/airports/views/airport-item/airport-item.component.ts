import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Airport } from '../../model/airport';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { AirportService } from '../../services/airport.service';

@Component({
  selector: 'app-airports-item',
  templateUrl:
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
})
export class AirportItemComponent
  extends CrudItemItemComponent<Airport>
  implements OnInit
{
  constructor(
    protected store: Store<AppState>,
    protected injector: Injector,
    public airportService: AirportService,
    protected layoutService: BiaClassicLayoutService
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
