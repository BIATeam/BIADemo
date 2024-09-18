import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { Confirmation, ConfirmationService } from 'primeng/api';
import { Table } from 'primeng/table';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';
import { View } from '../../model/view';

@Component({
  selector: 'bia-view-user-table',
  templateUrl: './view-user-table.component.html',
  styleUrls: ['./view-user-table.component.scss'],
})
export class ViewUserTableComponent implements OnChanges {
  @Input() views: View[];
  @Input() canDelete = false;
  @Input() canSetDefault = false;
  @Input() canUpdate = false;

  get viewSelected(): View | undefined {
    if (this.table) {
      return this.table.selection as View;
    }
    return undefined;
  }
  set viewSelected(value: View | undefined) {
    if (this.table) {
      this.table.selection = value;
    }
  }

  @ViewChild('viewUserTable', { static: false }) table: Table;

  @Output() delete = new EventEmitter<number>();
  @Output() setDefault = new EventEmitter<{
    viewId: number | undefined;
    isDefault: boolean;
  }>();
  @Output() viewSelect = new EventEmitter<View | undefined>();

  constructor(
    protected biaDialogService: BiaDialogService,
    protected confirmationService: ConfirmationService
  ) {}

  ngOnChanges(changes: SimpleChanges) {
    this.onViewsChange(changes);
  }

  onDeleteView(viewId: number | undefined) {
    if (!viewId) {
      return;
    }
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation('view-user-confirm'),
      accept: () => {
        this.delete.emit(viewId);
      },
    };
    this.confirmationService.confirm(confirmation);
  }

  onSetDefaultView(viewId: number | undefined, isDefault: boolean) {
    this.setDefault.emit({ viewId, isDefault });
  }

  onSelectionChange() {
    this.viewSelect.next(this.viewSelected);
  }

  showDefineDefault() {
    return !(
      this.viewSelected &&
      this.viewSelected.isUserDefault === true &&
      this.canSetDefault === true
    );
  }

  protected onViewsChange(changes: SimpleChanges) {
    if (changes.views && this.table) {
      const viewSelected: View | undefined = this.viewSelected;
      if (viewSelected && viewSelected.id > 0 && this.views) {
        this.viewSelected = this.views.filter(x => x.id === viewSelected.id)[0];
      } else {
        this.viewSelected = {} as View;
      }
      this.onSelectionChange();
    }
  }
}
