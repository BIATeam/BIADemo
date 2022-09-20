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
import { FormBuilder, FormGroup } from '@angular/forms';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { SiteAdvancedFilter } from '../../model/site-advanced-filter';

@Component({
  selector: 'app-site-filter',
  templateUrl: './site-filter.component.html',
  styleUrls: ['./site-filter.component.scss']
})
export class SiteFilterComponent implements OnInit, OnChanges {
  @ViewChild('template', { static: true }) template: TemplateRef<HTMLElement>;
  @Input() fxFlexValue: string;
  @Input() userOptions: OptionDto[];
  @Input() hidden = false;
  @Input() advancedFilter: SiteAdvancedFilter;
  @Output() closeFilter = new EventEmitter();
  @Output() searchUsers = new EventEmitter<string>();
  @Output() filter = new EventEmitter<SiteAdvancedFilter>();

  firstChange = true;
  form: FormGroup;
  
  constructor(public formBuilder: FormBuilder,private viewContainerRef: ViewContainerRef) {
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
  }

  onClose() {
    this.closeFilter.emit();
  }

  onReset() {
    this.form.reset();
    this.onFilter();
  }

  onFilter() {
    if (this.form.valid) {
      const vm = this.form.value;
      const advancedFilter = <SiteAdvancedFilter>{
        userId: vm.userSelected ? vm.userSelected.id : 0
      };
      this.filter.emit(advancedFilter);
    }
  }

  onSearchUsers(event: any) {
    this.searchUsers.emit(event.query);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.advancedFilter && this.allDataLoaded() && (this.firstChange === true || changes.advancedFilter)) {
      this.firstChange = false;
      const vm = {
        userSelected: this.advancedFilter.userId
          ? this.userOptions.find((x) => this.advancedFilter.userId == x.id)
          : null,
      };
      this.form.patchValue({ ...vm });
    }
  }
  private allDataLoaded(): boolean {
    return (
      this.userOptions &&
      this.userOptions.length > 0
    );
  }
}
