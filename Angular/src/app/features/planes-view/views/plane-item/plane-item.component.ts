import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { getCurrentPlane} from '../../store/plane.state';
import { Plane } from '../../model/plane';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute } from '@angular/router';
import { PlaneService } from '../../services/plane.service';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';

@Component({
  templateUrl: './plane-item.component.html',
  styleUrls: ['./plane-item.component.scss']
})
export class PlaneItemComponent implements OnInit, OnDestroy {
  plane$: Observable<Plane>;
  private sub = new Subscription();

  constructor(private store: Store<AppState>,
    private route: ActivatedRoute,
    public planeService: PlaneService,
    private layoutService: BiaClassicLayoutService) { }

  ngOnInit() {
    this.planeService.currentPlaneId = this.route.snapshot.params.planeId;
    this.sub.add
      (
        this.store.select(getCurrentPlane).subscribe((plane) => {
          if (plane?.msn) {
            this.route.data.pipe(first()).subscribe(routeData => {
              routeData['breadcrumb'] = plane.msn;
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
