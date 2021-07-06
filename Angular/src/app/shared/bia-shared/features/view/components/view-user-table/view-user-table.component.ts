import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { View } from '../../model/view';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-view-user-table',
  templateUrl: './view-user-table.component.html',
  styleUrls: ['./view-user-table.component.scss']
})
export class ViewUserTableComponent implements OnChanges {
  @Input() views: View[];
  @Input() canDelete = false;
  @Input() canSetDefault = false;
  @Input() canUpdate = false;

  get viewSelected(): View {
    if (this.table) {
      return this.table.selection as View;
    }
    return {} as View;
  }
  set viewSelected(value: View) {
    if (this.table) {
      this.table.selection = value;
    }
  }

  @ViewChild('viewUserTable', { static: false }) table: Table;

  @Output() delete = new EventEmitter<number>();
  @Output() setDefault = new EventEmitter<{ viewId: number; isDefault: boolean }>();
  @Output() viewSelect = new EventEmitter<View>();

  constructor() {}

  ngOnChanges(changes: SimpleChanges) {
    this.onViewsChange(changes);
  }

  onDeleteView(viewId: number) {
    this.delete.emit(viewId);
  }

  onSetDefaultView(viewId: number, isDefault: boolean) {
    this.setDefault.emit({ viewId, isDefault });
  }

  onSelectionChange() {
    this.viewSelect.next(this.viewSelected);
  }

  showDefineDefault() {
    return !(this.viewSelected && this.viewSelected.isUserDefault === true && this.canSetDefault === true);
  }

  private onViewsChange(changes: SimpleChanges) {
    if (changes.views && this.table) {
      if (this.viewSelected && this.viewSelected.id > 0 && this.views) {
        this.viewSelected = this.views.filter((x) => x.id === this.viewSelected.id)[0];
      } else {
        this.viewSelected = {} as View;
      }
      this.onSelectionChange();
    }
  }
}
