import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
} from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { TeamTypeId } from 'src/app/shared/constants';
import { Plane } from '../../model/plane';
import { InputText } from 'primeng/inputtext';
import { Checkbox } from 'primeng/checkbox';
import { DatePicker } from 'primeng/datepicker';
import { Select } from 'primeng/select';
import { MultiSelect } from 'primeng/multiselect';
import { ButtonDirective } from 'primeng/button';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-plane-form',
  templateUrl: './plane-form.component.html',
  styleUrls: ['./plane-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    InputText,
    Checkbox,
    DatePicker,
    Select,
    MultiSelect,
    ButtonDirective,
    TranslateModule,
  ],
})
export class PlaneFormComponent implements OnChanges {
  @Input() plane: Plane = <Plane>{};
  @Input() airportOptions: OptionDto[];
  @Input() planeTypeOptions: OptionDto[];

  @Output() save = new EventEmitter<Plane>();
  @Output() cancelled = new EventEmitter<void>();

  form: UntypedFormGroup;

  constructor(
    public formBuilder: UntypedFormBuilder,
    private authService: AuthService
  ) {
    this.initForm();
  }

  ngOnChanges() {
    if (this.plane) {
      this.form.reset();
      if (this.plane) {
        this.form.patchValue({ ...this.plane });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.plane.id],
      msn: [this.plane.msn, Validators.required],
      isActive: [this.plane.isActive],
      lastFlightDate: [this.plane.lastFlightDate],
      deliveryDate: [this.plane.deliveryDate],
      syncTime: [this.plane.syncTime],
      capacity: [this.plane.capacity, Validators.required],
      connectingAirports: [this.plane.connectingAirports],
      planeType: [this.plane.planeType?.id],
    });
  }

  onCancel() {
    this.form.reset();
    this.cancelled.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const plane: Plane = <Plane>this.form.value;
      plane.id = plane.id > 0 ? plane.id : 0;
      plane.isActive = plane.isActive ? plane.isActive : false;
      plane.connectingAirports = BiaOptionService.differential(
        plane.connectingAirports,
        this.plane?.connectingAirports
      );
      plane.planeType = BiaOptionService.clone(plane.planeType);

      // force the parent key => siteId from authService or other Id from 'parent'Service
      (plane.siteId = this.authService.getCurrentTeamId(TeamTypeId.Site)),
        this.save.emit(plane);
      this.form.reset();
    }
  }
}
