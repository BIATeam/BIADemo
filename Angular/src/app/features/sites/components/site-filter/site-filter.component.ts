import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  ViewChild,
  ViewContainerRef,
  TemplateRef,
  Input,
  SimpleChanges,
  OnChanges
} from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { getAllUserOptions } from 'src/app/domains/bia-domains/user-option/store/user-option.state';
import { DomainUserOptionsActions } from 'src/app/domains/bia-domains/user-option/store/user-options-actions';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { AppState } from 'src/app/store/state';
import { SiteAdvancedFilter } from '../../model/site-advanced-filter';

@Component({
  selector: 'app-site-filter',
  templateUrl: './site-filter.component.html',
  styleUrls: ['./site-filter.component.scss']
})
export class SiteFilterComponent implements OnInit, OnChanges {
  @ViewChild('template', { static: true }) template: TemplateRef<HTMLElement>;
  @Input() hidden = false;
  @Input() advancedFilter: SiteAdvancedFilter;
  @Output() closeFilter = new EventEmitter<void>();
  @Output() searchUsers = new EventEmitter<string>();
  @Output() filter = new EventEmitter<SiteAdvancedFilter>();

  userOptions$: Observable<OptionDto[]>;
  form: UntypedFormGroup;
  
  constructor(public formBuilder: UntypedFormBuilder,
    private viewContainerRef: ViewContainerRef,
    protected store: Store<AppState>) {
    this.initForm();
  }

  private initForm() {
    this.form = this.formBuilder.group({
      userSelected: null,
    });
  }

  ngOnInit() {
    // The goal here is that the html content of the component is not contained in its tag "app-user-filter".
    this.viewContainerRef.createEmbeddedView(this.template);
    this.initUsers();
  }

  private initUsers() {
    this.store.dispatch(DomainUserOptionsActions.loadAll());
    this.userOptions$ = this.store.select(getAllUserOptions);
  }

  onClose() {
    this.closeFilter.emit();
  }

  onReset() {
    this.form.reset();
  }

  onFilter() {
    if (this.form.valid) {
      const vm = this.form.value;
      const advancedFilter = <SiteAdvancedFilter>{
        userId: vm.userSelected
      };
      this.filter.emit(advancedFilter);
    }
  }

  onSearchUsers(event: any) {
    this.searchUsers.emit(event.query);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.advancedFilter) {
        const vm = {
          userSelected: this.advancedFilter?.userId
        };
        this.form.patchValue({ ...vm });
    }
  }
}
