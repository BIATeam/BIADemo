import { AsyncPipe, NgIf } from '@angular/common';
import {
  Component,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
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
  imports: [NgIf, PlaneFormComponent, AsyncPipe, SpinnerComponent],
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
