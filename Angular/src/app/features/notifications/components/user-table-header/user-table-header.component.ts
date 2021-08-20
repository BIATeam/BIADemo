import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';

@Component({
  selector: 'app-user-table-header',
  templateUrl: './user-table-header.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/components/table/bia-table-header/bia-table-header.component.scss',
    './user-table-header.component.scss'
  ],
  providers: [ConfirmationService]
})
export class UserTableHeaderComponent extends BiaTableHeaderComponent {
  @Input() canSync = false;
  @Output() synchronize = new EventEmitter();
}
