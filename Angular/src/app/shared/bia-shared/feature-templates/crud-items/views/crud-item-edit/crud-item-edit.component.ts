import { HttpStatusCode } from '@angular/common/http';
import {
  Component,
  EventEmitter,
  Injector,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Actions } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { Subscription, filter, first } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { AuthInfo } from 'src/app/shared/bia-shared/model/auth-info';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { AppState } from 'src/app/store/state';
import { CrudConfig, FormReadOnlyMode } from '../../model/crud-config';
import { CrudItemSingleService } from '../../services/crud-item-single.service';

@Component({
  selector: 'bia-crud-item-edit',
  templateUrl: './crud-item-edit.component.html',
  styleUrls: ['./crud-item-edit.component.scss'],
})
export class CrudItemEditComponent<CrudItem extends BaseDto>
  implements OnInit, OnDestroy
{
  @Output() displayChange = new EventEmitter<boolean>();
  protected sub = new Subscription();
  public crudConfiguration: CrudConfig<CrudItem>;
  public formReadOnlyMode: FormReadOnlyMode;
  public canFix: boolean;

  protected store: Store<AppState>;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected biaTranslationService: BiaTranslationService;
  protected actions: Actions;
  protected isCrudItemOutdated: boolean;
  protected authService: AuthService;

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>
  ) {
    this.store = this.injector.get<Store<AppState>>(Store);
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
    this.biaTranslationService = this.injector.get<BiaTranslationService>(
      BiaTranslationService
    );
    this.actions = this.injector.get<Actions>(Actions);
    this.authService = this.injector.get<AuthService>(AuthService);
  }

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;
    this.formReadOnlyMode =
      snapshot.data['readOnlyMode'] ?? FormReadOnlyMode.off;

    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.crudItemService.optionsService.loadAllOptions(
          this.crudConfiguration.optionFilter
        );
      })
    );

    this.sub.add(
      this.authService.authInfo$.subscribe((authInfo: AuthInfo) => {
        if (authInfo && authInfo.token !== '') {
          this.setPermissions();
        }
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(crudItemToUpdate: CrudItem) {
    const successActionType = this.crudItemService.updateSuccessActionType;
    const failureActionType = this.crudItemService.updateFailureActionType;

    if (successActionType) {
      this.actions
        .pipe(
          filter((action: any) => action.type === successActionType),
          first()
        )
        .subscribe(() => {
          this.navigateBack();
        });
    }

    if (failureActionType) {
      this.actions
        .pipe(
          filter((action: any) => action.type === failureActionType),
          first()
        )
        .subscribe(action => {
          if (action.error?.status === HttpStatusCode.Conflict) {
            this.isCrudItemOutdated = true;
          }
        });
    }

    this.crudItemService.update(crudItemToUpdate);

    if (!successActionType) {
      this.navigateBack();
    }
  }

  onCancelled() {
    this.navigateBack();
  }

  private navigateBack() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  protected setPermissions(): void {
    this.canFix = false;
  }
}
