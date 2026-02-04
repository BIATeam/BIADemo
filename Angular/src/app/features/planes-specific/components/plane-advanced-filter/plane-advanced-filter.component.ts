import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  TemplateRef,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
} from '@angular/forms';
import {
  DomainUserOptionsActions,
  UserOptionModule,
} from '@bia-team/bia-ng/domains';
import { OptionDto } from '@bia-team/bia-ng/models';
import { DtoState } from '@bia-team/bia-ng/models/enum';
import { BiaAppState } from '@bia-team/bia-ng/store';
import { Store } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import { ButtonDirective } from 'primeng/button';
import { FloatLabel } from 'primeng/floatlabel';
import { Select } from 'primeng/select';
import { Tooltip } from 'primeng/tooltip';
import { PlaneAdvancedFilterDto } from '../../model/plane-advanced-filter-dto';

@Component({
  selector: 'app-plane-advanced-filter',
  templateUrl: './plane-advanced-filter.component.html',
  styleUrls: ['./plane-advanced-filter.component.scss'],
  imports: [
    ButtonDirective,
    Tooltip,
    FormsModule,
    ReactiveFormsModule,
    Select,
    TranslateModule,
    FloatLabel,
    UserOptionModule,
  ],
})
export class PlaneAdvancedFilterComponent implements OnInit, OnChanges {
  @ViewChild('template', { static: true }) template: TemplateRef<HTMLElement>;
  @Input() hidden = false;
  @Input() advancedFilter: PlaneAdvancedFilterDto;
  @Output() closeFilter = new EventEmitter<void>();
  @Output() filter = new EventEmitter<PlaneAdvancedFilterDto>();

  enginesNumberRangeOptions: OptionDto[] = [
    new OptionDto(0, 'plane.enginesNumberRange.zero', DtoState.Unchanged),
    new OptionDto(1, 'plane.enginesNumberRange.oneOrTwo', DtoState.Unchanged),
    new OptionDto(
      2,
      'plane.enginesNumberRange.threeToFive',
      DtoState.Unchanged
    ),
    new OptionDto(3, 'plane.enginesNumberRange.sixOrMore', DtoState.Unchanged),
  ];
  form: UntypedFormGroup;
  submittingForm = false;

  constructor(
    public formBuilder: UntypedFormBuilder,
    protected viewContainerRef: ViewContainerRef,
    protected store: Store<BiaAppState>
  ) {
    this.initForm();
  }

  protected initForm() {
    this.form = this.formBuilder.group({
      enginesNumberRangeSelected: null,
    });
  }

  ngOnInit() {
    // The goal here is that the html content of the component is not contained in its tag "app-user-filter".
    this.viewContainerRef.createEmbeddedView(this.template);
    this.initUsers();
  }

  protected initUsers() {
    this.store.dispatch(DomainUserOptionsActions.loadAll());
  }

  onClose() {
    this.closeFilter.emit();
  }

  onReset() {
    this.form.reset();
  }

  onFilter() {
    if (this.form.valid) {
      this.submittingForm = true;
      const vm = this.form.value;
      const advancedFilter = <PlaneAdvancedFilterDto>{
        enginesNumberRange: vm.enginesNumberRangeSelected,
      };
      this.filter.emit(advancedFilter);
      setTimeout(() => {
        this.submittingForm = false;
      }, 2000);
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.advancedFilter) {
      const vm = {
        enginesNumberRange: this.advancedFilter?.enginesNumberRange,
      };
      this.form.patchValue({ ...vm });
    }
  }
}
