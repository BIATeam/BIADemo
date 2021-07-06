import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { create, closeDialogNew } from '../../store/sites-actions';
import { Site } from '../../model/site/site';
import { AppState } from 'src/app/store/state';
import { Subscription } from 'rxjs';
import { getDisplayNewDialog } from '../../store/site.state';

@Component({
  selector: 'app-site-new-dialog',
  templateUrl: './site-new-dialog.component.html',
  styleUrls: ['./site-new-dialog.component.scss']
})
export class SiteNewDialogComponent implements OnInit, OnDestroy {
  display = false;
  site: Site;
  private sub = new Subscription();
  @Output() displayChange = new EventEmitter<boolean>();

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.sub.add(
      this.store
        .select(getDisplayNewDialog)
        .subscribe((x) => (this.display = x))
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(siteToCreate: Site) {
    this.store.dispatch(create({ site: siteToCreate }));
    this.close();
  }

  onCancelled() {
    this.site = <Site>{};
    this.close();
  }

  public close() {
    this.store.dispatch(closeDialogNew());
  }
}
