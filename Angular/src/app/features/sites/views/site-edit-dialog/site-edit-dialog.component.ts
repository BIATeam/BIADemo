import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { update, closeDialogEdit } from '../../store/sites-actions';
import { Observable, Subscription } from 'rxjs';
import { getCurrentSite, getDisplayEditDialog, getSiteLoadingGet } from '../../store/site.state';
import { Site } from '../../model/site/site';
import { AppState } from 'src/app/store/state';

@Component({
  selector: 'app-site-edit-dialog',
  templateUrl: './site-edit-dialog.component.html',
  styleUrls: ['./site-edit-dialog.component.scss']
})
export class SiteEditDialogComponent implements OnInit, OnDestroy {
  loading$: Observable<boolean>;
  site$: Observable<Site>;
  display = false;
  private sub = new Subscription();
  @Output() displayChange = new EventEmitter<boolean>();

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.loading$ = this.store.select(getSiteLoadingGet).pipe();
    this.site$ = this.store.select(getCurrentSite).pipe();
    this.sub.add(
      this.store
        .select(getDisplayEditDialog)
        .subscribe((x) => (this.display = x))
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(siteToUpdate: Site) {
    this.store.dispatch(update({ site: siteToUpdate }));
    this.close();
  }

  onCancelled() {
    this.close();
  }

  close() {
    this.store.dispatch(closeDialogEdit());
  }
}
