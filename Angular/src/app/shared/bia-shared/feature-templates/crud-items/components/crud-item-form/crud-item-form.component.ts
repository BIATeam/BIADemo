import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

@Component({
  selector: 'app-crud-item-form',
  templateUrl: './crud-item-form.component.html',
  styleUrls: ['./crud-item-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class CrudItemFormComponent<CrudItem extends BaseDto> implements OnChanges {
  @Input() crudItem: CrudItem = <CrudItem>{};

  @Output() save = new EventEmitter<CrudItem>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(
      public formBuilder: FormBuilder,
      // protected authService: AuthService
    ) {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.crudItem) {
      this.form.reset();
      if (this.crudItem) {
        this.form.patchValue({ ...this.crudItem });
      }
    }
  }

  protected initForm() {
    // TODO redefine in plane 
    /*
    this.form = this.formBuilder.group({
      id: [this.crudItem.id],
      msn: [this.crudItem.msn, Validators.required],
      isActive: [this.crudItem.isActive],
      lastFlightDate: [this.crudItem.lastFlightDate],
      deliveryDate: [this.crudItem.deliveryDate],
      syncTime: [this.crudItem.syncTime],
      capacity: [this.crudItem.capacity, Validators.required],
      connectingAirports: [this.crudItem.connectingAirports],
      crudItemType: [this.crudItem.crudItemType?.id],
    });
    */

  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const crudItem: CrudItem = <CrudItem>this.form.value;
      crudItem.id = crudItem.id > 0 ? crudItem.id : 0;
      // TODO redefine in plane 
      /*
      crudItem.isActive = crudItem.isActive ? crudItem.isActive : false;
      crudItem.connectingAirports = BiaOptionService.Differential(crudItem.connectingAirports, this.crudItem?.connectingAirports);
      crudItem.crudItemType = BiaOptionService.Clone(crudItem.crudItemType);

      // force the parent key => siteId from authService or other Id from 'parent'Service
      crudItem.siteId = this.authService.getCurrentTeamId(TeamTypeId.Site),*/
      this.save.emit(crudItem);
      this.form.reset();
    }
  }
}

