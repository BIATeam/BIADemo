import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { update, closeDialogEdit } from '../../store/airports-actions';
import { Observable, Subscription } from 'rxjs';
import { getCurrentAirport, getDisplayEditDialog, getAirportLoadingGet } from '../../store/airport.state';
import { Airport } from '../../model/airport';
import { AppState } from 'src/app/store/state';

@Component({
  selector: 'app-airport-edit-dialog',
  templateUrl: './airport-edit-dialog.component.html',
  styleUrls: ['./airport-edit-dialog.component.scss']
})
export class AirportEditDialogComponent implements OnInit, OnDestroy {
  loading$: Observable<boolean>;
  airport$: Observable<Airport>;
  display = false;
  private sub = new Subscription();
  @Output() displayChange = new EventEmitter<boolean>();

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.loading$ = this.store.select(getAirportLoadingGet).pipe();
    this.airport$ = this.store.select(getCurrentAirport).pipe();
    this.sub.add(
      this.store
        .select(getDisplayEditDialog)
        .pipe()
        .subscribe((x) => (this.display = x))
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(airportToUpdate: Airport) {
    this.store.dispatch(update({ airport: airportToUpdate }));
    this.close();
  }

  onCancelled() {
    this.close();
  }

  close() {
    this.store.dispatch(closeDialogEdit());
  }
}
