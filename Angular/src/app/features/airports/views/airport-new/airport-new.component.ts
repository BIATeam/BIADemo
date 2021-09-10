import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { create } from '../../store/airports-actions';
import { Airport } from '../../model/airport';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-airport-new',
  templateUrl: './airport-new.component.html',
  styleUrls: ['./airport-new.component.scss']
})
export class AirportNewComponent implements OnInit {

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) {}

  ngOnInit() {
  }

  onSubmitted(airportToCreate: Airport) {
    this.store.dispatch(create({ airport: airportToCreate }));
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
