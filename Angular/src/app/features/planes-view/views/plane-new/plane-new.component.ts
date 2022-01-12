import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { create } from '../../store/planes-actions';
import { Plane } from '../../model/plane';
import { AppState } from 'src/app/store/state';
import { Location } from '@angular/common';

@Component({
  selector: 'app-plane-new',
  templateUrl: './plane-new.component.html',
  styleUrls: ['./plane-new.component.scss']
})
export class PlaneNewComponent {
  plane: Plane;

  constructor(private store: Store<AppState>, private location: Location) {}

  onSubmitted(planeToCreate: Plane) {
    this.store.dispatch(create({ plane: planeToCreate }));
    this.location.back();
  }

  onCancelled() {
    this.plane = <Plane>{};
    this.location.back();
  }
}
