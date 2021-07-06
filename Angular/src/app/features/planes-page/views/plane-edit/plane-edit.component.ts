import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { update } from '../../store/planes-actions';
import { Subscription } from 'rxjs';
import { Plane } from '../../model/plane';
import { AppState } from 'src/app/store/state';
import { Location } from '@angular/common';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-edit',
  templateUrl: './plane-edit.component.html',
  styleUrls: ['./plane-edit.component.scss']
})
export class PlaneEditComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  private sub = new Subscription();

  constructor(
    private store: Store<AppState>,
    private location: Location,
    public planeService: PlaneService
  ) { }

  ngOnInit() {
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(planeToUpdate: Plane) {
    this.store.dispatch(update({ plane: planeToUpdate }));
    this.location.back();
  }

  onCancelled() {
    this.location.back();
  }
}
