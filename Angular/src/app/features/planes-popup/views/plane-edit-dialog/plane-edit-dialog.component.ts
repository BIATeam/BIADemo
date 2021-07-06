import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { update, closeDialogEdit } from '../../store/planes-actions';
import { Observable, Subscription } from 'rxjs';
import { getCurrentPlane, getDisplayEditDialog, getPlaneLoadingGet } from '../../store/plane.state';
import { Plane } from '../../model/plane';
import { AppState } from 'src/app/store/state';
import { getAllAirportOptions } from 'src/app/domains/airport-option/store/airport-option.state';
import { loadAllAirportOptions } from 'src/app/domains/airport-option/store/airport-options-actions';
import { getAllPlaneTypeOptions } from 'src/app/domains/plane-type-option/store/plane-type-option.state';
import { loadAllPlaneTypeOptions } from 'src/app/domains/plane-type-option/store/plane-type-options-actions';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

@Component({
  selector: 'app-plane-edit-dialog',
  templateUrl: './plane-edit-dialog.component.html',
  styleUrls: ['./plane-edit-dialog.component.scss']
})
export class PlaneEditDialogComponent implements OnInit, OnDestroy {
  loading$: Observable<boolean>;
  plane$: Observable<Plane>;
  airportOptions$: Observable<OptionDto[]>;
  planeTypeOptions$: Observable<OptionDto[]>;

  display = false;
  private sub = new Subscription();
  @Output() displayChange = new EventEmitter<boolean>();

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.loading$ = this.store.select(getPlaneLoadingGet).pipe();
    this.plane$ = this.store.select(getCurrentPlane).pipe();

    this.sub.add(
      this.store
        .select(getDisplayEditDialog)
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

  onSubmitted(planeToUpdate: Plane) {
    this.store.dispatch(update({ plane: planeToUpdate }));
    this.close();
  }

  onCancelled() {
    this.close();
  }

  close() {
    this.store.dispatch(closeDialogEdit());
  }
}
