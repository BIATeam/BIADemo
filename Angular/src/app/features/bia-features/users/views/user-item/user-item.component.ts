import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { getCurrentUser} from '../../store/user.state';
import { User } from '../../model/user';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../services/user.service';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';

@Component({
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.scss']
})
export class UserItemComponent implements OnInit, OnDestroy {
  user$: Observable<User>;
  private sub = new Subscription();

  constructor(private store: Store<AppState>,
    private route: ActivatedRoute,
    public userService: UserService,
    private layoutService: BiaClassicLayoutService,
  ) { }

  ngOnInit() {
    this.userService.currentUserId = this.route.snapshot.params.userId;
    this.sub.add
      (
        this.store.select(getCurrentUser).subscribe((user) => {
          if (user?.displayName) {
            this.route.data.pipe(first()).subscribe(routeData => {
              (routeData as any)['breadcrumb'] = user.displayName;
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
