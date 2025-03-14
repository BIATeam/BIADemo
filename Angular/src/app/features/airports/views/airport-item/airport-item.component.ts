import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { Airport } from '../../model/airport';
import { AirportService } from '../../services/airport.service';
import { RouterOutlet } from '@angular/router';
import { NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';

@Component({
  selector: 'app-airports-item',
  templateUrl:
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, NgIf, BiaSharedModule, AsyncPipe, SpinnerComponent],
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
