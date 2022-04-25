import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  ViewChild,
  ViewContainerRef,
  TemplateRef,
  Input
} from '@angular/core';
import { User } from 'src/app/features/bia-features/users/model/user';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { SiteAdvancedFilter } from '../../model/site/site-advanced-filter';

@Component({
  selector: 'app-site-filter',
  templateUrl: './site-filter.component.html',
  styleUrls: ['./site-filter.component.scss']
})
export class SiteFilterComponent implements OnInit {
  @ViewChild('template', { static: true }) template: TemplateRef<HTMLElement>;
  @Input() fxFlexValue: string;
  @Input() userOptions: OptionDto[];
  @Input() hidden = false;
  @Output() closeFilter = new EventEmitter();
  @Output() searchUsers = new EventEmitter<string>();
  @Output() filter = new EventEmitter<SiteAdvancedFilter>();

  userSelected: User;

  constructor(private viewContainerRef: ViewContainerRef) {}

  ngOnInit() {
    // The goal here is that the html content of the component is not contained in its tag "app-user-filter".
    this.viewContainerRef.createEmbeddedView(this.template);
  }

  onClose() {
    this.closeFilter.emit();
  }

  onReset() {
    this.userSelected = <User>{};
    this.onFilter();
  }

  onFilter() {
    const advancedFilter = <SiteAdvancedFilter>{
      userId: this.userSelected ? this.userSelected.id : 0
    };
    this.filter.emit(advancedFilter);
  }

  onSearchUsers(event: any) {
    this.searchUsers.emit(event.query);
  }
}
