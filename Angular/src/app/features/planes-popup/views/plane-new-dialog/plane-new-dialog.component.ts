import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { create, closeDialogNew } from '../../store/planes-actions';
import { Plane } from '../../model/plane';
import { AppState } from 'src/app/store/state';
import { Observable, Subscription } from 'rxjs';
import { getDisplayNewDialog } from '../../store/plane.state';
import { getAllAirportOptions } from 'src/app/domains/airport-option/store/airport-option.state';
import { loadAllAirportOptions } from 'src/app/domains/airport-option/store/airport-options-actions';
import { getAllPlaneTypeOptions } from 'src/app/domains/plane-type-option/store/plane-type-option.state';
import { loadAllPlaneTypeOptions } from 'src/app/domains/plane-type-option/store/plane-type-options-actions';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

@Component({
  selector: 'app-plane-new-dialog',
  templateUrl: './plane-new-dialog.component.html',
  styleUrls: ['./plane-new-dialog.component.scss']
})
export class PlaneNewDialogComponent implements OnInit, OnDestroy {
  display = false;
  plane: Plane;
  airportOptions$: Observable<OptionDto[]>;
  planeTypeOptions$: Observable<OptionDto[]>;

  private sub = new Subscription();
  @Output() displayChange = new EventEmitter<boolean>();

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.sub.add(
      this.store
        .select(getDisplayNewDialog)
        .pipe()
        .subscribe((x) => (this.display = x))
    );
    this.airportOptions$ = this.store.select(getAllAirportOptions).pipe();
    this.store.dispatch(loadAllAirportOptions());
    this.planeTypeOptions$ = this.store.select(getAllPlaneTypeOptions).pipe();
    this.store.dispatch(loadAllPlaneTypeOptions());
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(planeToCreate: Plane) {
    this.store.dispatch(create({ plane: planeToCreate }));
    this.close();
  }

  onCancelled() {
    this.plane = <Plane>{};
    this.close();
  }

  public close() {
    this.store.dispatch(closeDialogNew());
  }
}
