import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService, BiaOptionService } from 'packages/bia-ng/core/public-api';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { ButtonDirective } from 'primeng/button';
import { Checkbox } from 'primeng/checkbox';
import { DatePicker } from 'primeng/datepicker';
import { FloatLabel } from 'primeng/floatlabel';
import { InputText } from 'primeng/inputtext';
import { MultiSelect } from 'primeng/multiselect';
import { Select } from 'primeng/select';
import { TeamTypeId } from 'src/app/shared/constants';
import { Plane } from '../../model/plane';

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
    FloatLabel,
  ],
})
export class PlaneFormComponent implements OnChanges {
  @Input() plane: Plane = <Plane>{};
  @Input() airportOptions: OptionDto[];
  @Input() planeTypeOptions: OptionDto[];

  @Output() save = new EventEmitter<Plane>();
  @Output() cancelled = new EventEmitter<void>();

  form: UntypedFormGroup;
  submittingForm = false;

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
      this.submittingForm = true;
      const plane: Plane = <Plane>this.form.value;
      plane.id = plane.id > 0 ? plane.id : 0;
      plane.isActive = plane.isActive ? plane.isActive : false;
      plane.connectingAirports = BiaOptionService.differential(
        plane.connectingAirports,
        this.plane?.connectingAirports
      );
      plane.planeType = BiaOptionService.clone(plane.planeType);

      // force the parent key => siteId from authService or other Id from 'parent'Service
      plane.siteId = this.authService.getCurrentTeamId(TeamTypeId.Site);
      this.save.emit(plane);
      this.form.reset();
      setTimeout(() => {
        this.submittingForm = false;
      }, 2000);
    }
  }
}
