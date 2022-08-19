import {
  ChangeDetectionStrategy,
  Component,
  Input,
} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { TeamTypeId } from 'src/app/shared/constants';
import { Plane } from '../../model/plane';

@Component({
  selector: 'app-plane-form',
  templateUrl: './plane-form.component.html',
  styleUrls: ['./plane-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class PlaneFormComponent extends CrudItemFormComponent<Plane> {
  @Input() airportOptions: OptionDto[];
  @Input() planeTypeOptions: OptionDto[];

  constructor(public formBuilder: FormBuilder,
    protected authService: AuthService) {
    super(formBuilder);
    this.initForm();
  }

  protected initForm() {
    this.form = this.formBuilder.group({
      id: [this.crudItem.id],
      msn: [this.crudItem.msn, Validators.required],
      isActive: [this.crudItem.isActive],
      lastFlightDate: [this.crudItem.lastFlightDate],
      deliveryDate: [this.crudItem.deliveryDate],
      syncTime: [this.crudItem.syncTime],
      capacity: [this.crudItem.capacity, Validators.required],
      connectingAirports: [this.crudItem.connectingAirports],
      planeType: [this.crudItem.planeType?.id],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const plane: Plane = <Plane>this.form.value;
      plane.id = plane.id > 0 ? plane.id : 0;
      plane.isActive = plane.isActive ? plane.isActive : false;
      plane.connectingAirports = BiaOptionService.Differential(plane.connectingAirports, this.crudItem?.connectingAirports);
      plane.planeType = BiaOptionService.Clone(plane.planeType);

      // force the parent key => siteId from authService or other Id from 'parent'Service
      plane.siteId = this.authService.getCurrentTeamId(TeamTypeId.Site),
      this.save.emit(plane);
      this.form.reset();
    }
  }
}

