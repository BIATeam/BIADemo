import { AsyncPipe, NgIf } from '@angular/common';
import { HttpStatusCode } from '@angular/common/http';
import {
  Component,
  EventEmitter,
  Injector,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { Subscription, filter, first, tap } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { AuthInfo } from 'src/app/shared/bia-shared/model/auth-info';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { CrudItemFormComponent } from '../../components/crud-item-form/crud-item-form.component';
import { FormReadOnlyMode } from '../../model/crud-config';
import { CrudItemSingleService } from '../../services/crud-item-single.service';
import { CrudItemComponent } from '../crud-item/crud-item.component';

@Component({
  selector: 'bia-crud-item-edit',
  templateUrl: './crud-item-edit.component.html',
  styleUrls: ['./crud-item-edit.component.scss'],
  imports: [NgIf, CrudItemFormComponent, SpinnerComponent, AsyncPipe],
})
export class CrudItemEditComponent<CrudItem extends BaseDto>
  extends CrudItemComponent<CrudItem>
  implements OnInit, OnDestroy
{
  @Output() displayChange = new EventEmitter<boolean>();
  protected permissionSub = new Subscription();
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
  }

  onSubmitted(crudItemToUpdate: CrudItem) {
    const successActionType = this.crudItemService.updateSuccessActionType;
    const failureActionType = this.crudItemService.updateFailureActionType;

    if (successActionType) {
      this.actions
        .pipe(
          filter(
            (action: any) =>
              action.type === successActionType ||
              action.type === biaSuccessWaitRefreshSignalR.type
          ),
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

  protected navigateBack() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  protected setPermissions(): void {
    this.permissionSub.unsubscribe();
    this.permissionSub = new Subscription();
    this.canFix = false;
  }
}
