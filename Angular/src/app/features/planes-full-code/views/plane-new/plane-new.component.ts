import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { AppState } from 'src/app/store/state';
import { Plane } from '../../model/plane';
import { PlaneOptionsService } from '../../services/plane-options.service';
import { FeaturePlanesActions } from '../../store/planes-actions';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-plane-new',
  templateUrl: './plane-new.component.html',
  styleUrls: ['./plane-new.component.scss'],
  imports: [PlaneFormComponent, AsyncPipe],
})
export class PlaneNewComponent implements OnInit, OnDestroy {
  private sub = new Subscription();

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public planeOptionsService: PlaneOptionsService,
    private biaTranslationService: BiaTranslationService
  ) {}

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.planeOptionsService.loadAllOptions();
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(planeToCreate: Plane) {
    this.store.dispatch(FeaturePlanesActions.create({ plane: planeToCreate }));
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
