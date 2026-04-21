import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  TemplateRef,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { BiaAdvancedFilterComponent } from 'packages/bia-ng/shared/components/public-api';
import {
  PlaneAdvancedFilterDto,
  planeAdvancedFilterConfig,
} from '../../model/plane-advanced-filter-dto';

@Component({
  selector: 'app-plane-advanced-filter',
  templateUrl: './plane-advanced-filter.component.html',
  styleUrls: ['./plane-advanced-filter.component.scss'],
  imports: [BiaAdvancedFilterComponent],
})
export class PlaneAdvancedFilterComponent implements OnInit {
  @ViewChild('template', { static: true }) template: TemplateRef<HTMLElement>;

  @Input() hidden = false;
  @Input() advancedFilter: PlaneAdvancedFilterDto;
  @Output() closeFilter = new EventEmitter<void>();
  @Output() filter = new EventEmitter<PlaneAdvancedFilterDto>();

  readonly filterConfig = planeAdvancedFilterConfig;

  constructor(protected viewContainerRef: ViewContainerRef) {}

  ngOnInit() {
    this.viewContainerRef.createEmbeddedView(this.template);
  }

  onFilter(value: PlaneAdvancedFilterDto) {
    this.filter.emit(value);
  }

  onClose() {
    this.closeFilter.emit();
  }
}
