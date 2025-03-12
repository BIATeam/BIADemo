import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { first } from 'rxjs/operators';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { AppState } from 'src/app/store/state';
import { Plane } from '../../model/plane';
import { PlaneService } from '../../services/plane.service';
import { getCurrentPlane } from '../../store/plane.state';
import { NgIf, AsyncPipe } from '@angular/common';
import { SpinnerComponent } from '../../../../shared/bia-shared/components/spinner/spinner.component';

@Component({
    selector: 'app-planes-item',
    templateUrl: './plane-item.component.html',
    styleUrls: ['./plane-item.component.scss'],
    imports: [RouterOutlet, NgIf, SpinnerComponent, AsyncPipe]
})
export class PlaneItemComponent implements OnInit, OnDestroy {
  plane$: Observable<Plane>;
  private sub = new Subscription();

  constructor(
    private store: Store<AppState>,
    private route: ActivatedRoute,
    public planeService: PlaneService,
    private layoutService: BiaLayoutService
  ) {}

  ngOnInit() {
    this.planeService.currentPlaneId = this.route.snapshot.params.planeId;
    this.sub.add(
      this.store.select(getCurrentPlane).subscribe(plane => {
        if (plane?.msn) {
          this.route.data.pipe(first()).subscribe(routeData => {
            (routeData as any)['breadcrumb'] = plane.msn;
          });
          this.layoutService.refreshBreadcrumb();
        }
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
}
