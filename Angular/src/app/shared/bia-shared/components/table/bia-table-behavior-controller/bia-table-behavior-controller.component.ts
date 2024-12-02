import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CrudConfig } from '../../../feature-templates/crud-items/model/crud-config';

@Component({
  selector: 'bia-table-behavior-controller',
  templateUrl: './bia-table-behavior-controller.component.html',
  styleUrls: ['./bia-table-behavior-controller.component.scss'],
})
export class BiaTableBehaviorControllerComponent<TDto extends { id: number }> {
  @Input() crudConfiguration: CrudConfig<TDto>;

  @Output() useCalcModeChanged = new EventEmitter<boolean>();
  @Output() usePopupChanged = new EventEmitter<boolean>();
  @Output() useSignalRChanged = new EventEmitter<boolean>();
  @Output() useViewChanged = new EventEmitter<boolean>();
  @Output() useCompactModeChanged = new EventEmitter<boolean>();
  @Output() useVirtualScrollChanged = new EventEmitter<boolean>();
  @Output() useResizableColumnChanged = new EventEmitter<boolean>();
}
