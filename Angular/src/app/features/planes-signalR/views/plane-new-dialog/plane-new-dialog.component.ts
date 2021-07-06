import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { create, closeDialogNew } from '../../store/planes-actions';
import { Plane } from '../../model/plane';
import { AppState } from 'src/app/store/state';
import { Subscription } from 'rxjs';
import { getDisplayNewDialog } from '../../store/plane.state';

@Component({
  selector: 'app-plane-new-dialog',
  templateUrl: './plane-new-dialog.component.html',
  styleUrls: ['./plane-new-dialog.component.scss']
})
export class PlaneNewDialogComponent implements OnInit, OnDestroy {
  display = false;
  plane: Plane;
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
