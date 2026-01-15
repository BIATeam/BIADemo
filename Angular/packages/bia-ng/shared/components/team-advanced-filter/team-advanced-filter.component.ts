import { AsyncPipe } from '@angular/common';
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
import { Store } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import {
  DomainUserOptionsActions,
  DomainUserOptionsStore,
  UserOptionModule,
} from 'packages/bia-ng/domains/public-api';
import {
  OptionDto,
  TeamAdvancedFilterDto,
} from 'packages/bia-ng/models/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { ButtonDirective } from 'primeng/button';
import { FloatLabel } from 'primeng/floatlabel';
import { Select } from 'primeng/select';
import { Tooltip } from 'primeng/tooltip';
import { Observable } from 'rxjs';

@Component({
  selector: 'bia-team-advanced-filter',
  templateUrl: './team-advanced-filter.component.html',
  styleUrls: ['./team-advanced-filter.component.scss'],
  imports: [
    ButtonDirective,
    Tooltip,
    FormsModule,
    ReactiveFormsModule,
    Select,
    AsyncPipe,
    TranslateModule,
    FloatLabel,
    UserOptionModule,
  ],
})
export class TeamAdvancedFilterComponent implements OnInit, OnChanges {
  @ViewChild('template', { static: true }) template: TemplateRef<HTMLElement>;
  @Input() hidden = false;
  @Input() advancedFilter: TeamAdvancedFilterDto;
  @Output() closeFilter = new EventEmitter<void>();
  @Output() searchUsers = new EventEmitter<string>();
  @Output() filter = new EventEmitter<TeamAdvancedFilterDto>();

  userOptions$: Observable<OptionDto[]>;
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
      userSelected: null,
    });
  }

  ngOnInit() {
    // The goal here is that the html content of the component is not contained in its tag "app-user-filter".
    this.viewContainerRef.createEmbeddedView(this.template);
    this.initUsers();
  }

  protected initUsers() {
    this.store.dispatch(DomainUserOptionsActions.loadAll());
    this.userOptions$ = this.store.select(
      DomainUserOptionsStore.getAllUserOptions
    );
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
      const advancedFilter = <TeamAdvancedFilterDto>{
        userId: vm.userSelected,
      };
      this.filter.emit(advancedFilter);
      setTimeout(() => {
        this.submittingForm = false;
      }, 2000);
    }
  }

  onSearchUsers(event: any) {
    this.searchUsers.emit(event.query);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.advancedFilter) {
      const vm = {
        userSelected: this.advancedFilter?.userId,
      };
      this.form.patchValue({ ...vm });
    }
  }
}
