import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { update } from '../../store/planes-actions';
import { Subscription } from 'rxjs';
import { Plane } from '../../model/plane';
import { AppState } from 'src/app/store/state';
import { PlaneService } from '../../services/plane.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PlaneOptionsService } from '../../services/plane-options.service';

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
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public planeOptionsService: PlaneOptionsService,
    public planeService: PlaneService,
  ) { }

  ngOnInit() {
    this.planeOptionsService.loadAllOptions();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(planeToUpdate: Plane) {
    this.store.dispatch(update({ plane: planeToUpdate }));
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }
}
