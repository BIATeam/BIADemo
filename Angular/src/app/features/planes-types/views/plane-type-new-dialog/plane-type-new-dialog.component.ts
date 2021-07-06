import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { create, closeDialogNew } from '../../store/planes-types-actions';
import { PlaneType } from '../../model/plane-type';
import { AppState } from 'src/app/store/state';
import { Subscription } from 'rxjs';
import { getDisplayNewDialog } from '../../store/plane-type.state';

@Component({
  selector: 'app-plane-type-new-dialog',
  templateUrl: './plane-type-new-dialog.component.html',
  styleUrls: ['./plane-type-new-dialog.component.scss']
})
export class PlaneTypeNewDialogComponent implements OnInit, OnDestroy {
  display = false;
  planeType: PlaneType;
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

  onSubmitted(planeTypeToCreate: PlaneType) {
    this.store.dispatch(create({ planeType: planeTypeToCreate }));
    this.close();
  }

  onCancelled() {
    this.planeType = <PlaneType>{};
    this.close();
  }

  public close() {
    this.store.dispatch(closeDialogNew());
  }
}
