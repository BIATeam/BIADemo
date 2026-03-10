import { AsyncPipe } from '@angular/common';
import { HttpStatusCode } from '@angular/common/http';
import {
  Component,
  EventEmitter,
  Injector,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import {
  AuthService,
  biaSuccessWaitRefreshSignalR,
} from 'packages/bia-ng/core/public-api';
import { AuthInfo, BaseDto } from 'packages/bia-ng/models/public-api';
import { Subscription, filter, tap } from 'rxjs';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { CrudItemFormComponent } from '../../components/crud-item-form/crud-item-form.component';
import { FormReadOnlyMode } from '../../model/crud-config';
import { CrudItemSingleService } from '../../services/crud-item-single.service';
import { CrudItemComponent } from '../crud-item/crud-item.component';

@Component({
  selector: 'bia-crud-item-edit',
  templateUrl: './crud-item-edit.component.html',
  styleUrls: ['./crud-item-edit.component.scss'],
  imports: [CrudItemFormComponent, SpinnerComponent, AsyncPipe],
})
export class CrudItemEditComponent<CrudItem extends BaseDto<string | number>>
  extends CrudItemComponent<CrudItem>
  implements OnInit, OnDestroy
{
  @Output() displayChange = new EventEmitter<boolean>();
  protected permissionSub = new Subscription();
  protected onSubmitSub = new Subscription();
  private _formReadOnlyMode: FormReadOnlyMode;
  protected set formReadOnlyMode(value: FormReadOnlyMode) {
    this._formReadOnlyMode = value;
  }
  public get formReadOnlyMode(): FormReadOnlyMode {
    return this._formReadOnlyMode;
  }
  protected initialFormReadOnlyMode: FormReadOnlyMode;
  public canFix: boolean;

  protected isCrudItemOutdated = false;
  protected authService: AuthService;

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>
  ) {
    super(injector, crudItemService);
    this.authService = this.injector.get<AuthService>(AuthService);
    this.sub.add(
      this.crudItemService.crudItem$
        .pipe(
          tap(() => {
            this.isCrudItemOutdated = false;
          })
        )
        .subscribe()
    );
  }

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;
    this.formReadOnlyMode =
      snapshot.data['readOnlyMode'] ?? FormReadOnlyMode.off;
    this.initialFormReadOnlyMode = this.formReadOnlyMode;

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

    this.permissionSub.unsubscribe();
    this.onSubmitSub.unsubscribe();
  }

  onSubmitted(crudItemToUpdate: CrudItem) {
    const successActionType = this.crudItemService.updateSuccessActionType;
    const failureActionType = this.crudItemService.updateFailureActionType;

    this.onSubmitSub.unsubscribe();
    this.onSubmitSub = this.actions
      .pipe(
        filter((action: any) => {
          return !!(
            (successActionType && action.type === successActionType) ||
            (failureActionType && action.type === failureActionType) ||
            action.type === biaSuccessWaitRefreshSignalR.type
          );
        })
      )
      .subscribe((action: any) => {
        if (
          (successActionType && action.type === successActionType) ||
          action.type === biaSuccessWaitRefreshSignalR.type
        ) {
          this.navigateBack();
        }
        if (failureActionType && action.type === failureActionType) {
          if (action.error?.status === HttpStatusCode.Conflict) {
            this.isCrudItemOutdated = true;
          }
        }
      });

    this.crudItemService.update(crudItemToUpdate);

    if (!successActionType) {
      this.navigateBack();
    }
  }

  protected navigateBack() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  protected setPermissions(): void {
    this.permissionSub.unsubscribe();
    this.permissionSub = new Subscription();
    this.canFix = false;
  }
}
