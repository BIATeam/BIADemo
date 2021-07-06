import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { create, closeDialogNew } from '../../store/airports-actions';
import { Airport } from '../../model/airport';
import { AppState } from 'src/app/store/state';
import { Subscription } from 'rxjs';
import { getDisplayNewDialog } from '../../store/airport.state';

@Component({
  selector: 'app-airport-new-dialog',
  templateUrl: './airport-new-dialog.component.html',
  styleUrls: ['./airport-new-dialog.component.scss']
})
export class AirportNewDialogComponent implements OnInit, OnDestroy {
  display = false;
  airport: Airport;
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
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(airportToCreate: Airport) {
    this.store.dispatch(create({ airport: airportToCreate }));
    this.close();
  }

  onCancelled() {
    this.airport = <Airport>{};
    this.close();
  }

  public close() {
    this.store.dispatch(closeDialogNew());
  }
}
