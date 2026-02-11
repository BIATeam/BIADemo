import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { BiaDialogService } from 'packages/bia-ng/core/public-api';
import { ViewType } from 'packages/bia-ng/models/enum/public-api';
import { Confirmation, ConfirmationService, PrimeTemplate } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { Table, TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { View } from '../../model/view';

@Component({
  selector: 'bia-view-user-table',
  templateUrl: './view-user-table.component.html',
  styleUrls: ['./view-user-table.component.scss'],
  imports: [
    ButtonDirective,
    Tooltip,
    TableModule,
    PrimeTemplate,
    ConfirmDialog,
    TranslateModule,
  ],
})
export class ViewUserTableComponent {
  @Input() views: View[];
  @Input() canDelete = false;
  @Input() canSetDefault = false;
  @Input() canUpdate = false;

  @ViewChild('viewUserTable', { static: false }) table: Table;

  @Output() delete = new EventEmitter<number>();
  @Output() setDefault = new EventEmitter<{
    viewId: number | undefined;
    isDefault: boolean;
  }>();
  @Output() viewSelect = new EventEmitter<View | undefined>();

  viewType: typeof ViewType = ViewType;

  constructor(
    protected biaDialogService: BiaDialogService,
    protected confirmationService: ConfirmationService
  ) {}

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

  toggleDefault(view: View) {
    this.setDefault.emit({ viewId: view.id, isDefault: !view.isUserDefault });
  }
}
