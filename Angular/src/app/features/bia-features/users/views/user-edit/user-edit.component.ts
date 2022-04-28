import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { FeatureUsersActions } from '../../store/users-actions';
import { Subscription } from 'rxjs';
import { User } from '../../model/user';
import { AppState } from 'src/app/store/state';
import { UserService } from '../../services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserOptionsService } from '../../services/user-options.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';

@Component({
  selector: 'bia-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss']
})
export class UserEditComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  private sub = new Subscription();

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public userOptionsService: UserOptionsService,
    public userService: UserService,
    private biaTranslationService: BiaTranslationService,
  ) { }

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(event => {
          this.userOptionsService.loadAllOptions();
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(userToUpdate: User) {
    this.store.dispatch(FeatureUsersActions.update({ user: userToUpdate }));
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }
}
