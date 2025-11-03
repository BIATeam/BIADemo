import { AsyncPipe } from '@angular/common';
import {
  Component,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { BiaTranslationService } from 'packages/bia-ng/core/public-api';
import { SpinnerComponent } from 'packages/bia-ng/shared/public-api';
import { Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { Plane } from '../../model/plane';
import { PlaneOptionsService } from '../../services/plane-options.service';
import { PlaneService } from '../../services/plane.service';
import { FeaturePlanesActions } from '../../store/planes-actions';

@Component({
  selector: 'app-plane-edit',
  templateUrl: './plane-edit.component.html',
  styleUrls: ['./plane-edit.component.scss'],
  imports: [PlaneFormComponent, AsyncPipe, SpinnerComponent],
})
export class PlaneEditComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  private sub = new Subscription();

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public planeOptionsService: PlaneOptionsService,
    public planeService: PlaneService,
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

  onSubmitted(planeToUpdate: Plane) {
    this.store.dispatch(FeaturePlanesActions.update({ plane: planeToUpdate }));
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }
}
