import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { create } from '../../store/planes-actions';
import { Plane } from '../../model/plane';
import { AppState } from 'src/app/store/state';
import { PlaneOptionsService } from '../../services/plane-options.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-plane-new',
  templateUrl: './plane-new.component.html',
  styleUrls: ['./plane-new.component.scss']
})
export class PlaneNewComponent implements OnInit {

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public planeOptionsService: PlaneOptionsService,
  ) {}

  ngOnInit() {
    this.planeOptionsService.loadAllOptions();
  }

  onSubmitted(planeToCreate: Plane) {
    this.store.dispatch(create({ plane: planeToCreate }));
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
