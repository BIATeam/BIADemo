import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { update, closeDialogEdit } from '../../store/planes-types-actions';
import { Observable, Subscription } from 'rxjs';
import { getCurrentPlaneType, getDisplayEditDialog, getPlaneTypeLoadingGet } from '../../store/plane-type.state';
import { PlaneType } from '../../model/plane-type';
import { AppState } from 'src/app/store/state';

@Component({
  selector: 'app-plane-type-edit-dialog',
  templateUrl: './plane-type-edit-dialog.component.html',
  styleUrls: ['./plane-type-edit-dialog.component.scss']
})
export class PlaneTypeEditDialogComponent implements OnInit, OnDestroy {
  loading$: Observable<boolean>;
  planeType$: Observable<PlaneType>;
  display = false;
  private sub = new Subscription();
  @Output() displayChange = new EventEmitter<boolean>();

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.loading$ = this.store.select(getPlaneTypeLoadingGet).pipe();
    this.planeType$ = this.store.select(getCurrentPlaneType).pipe();
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

  onSubmitted(planeTypeToUpdate: PlaneType) {
    this.store.dispatch(update({ planeType: planeTypeToUpdate }));
    this.close();
  }

  onCancelled() {
    this.close();
  }

  close() {
    this.store.dispatch(closeDialogEdit());
  }
}
