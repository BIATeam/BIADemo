import { Component, Input, Output, EventEmitter, OnChanges, ChangeDetectionStrategy } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { ConfirmationService, Confirmation } from 'primeng/api';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';

@Component({
  selector: 'bia-table-header',
  templateUrl: './bia-table-header.component.html',
  styleUrls: ['./bia-table-header.component.scss'],
  providers: [ConfirmationService],
  changeDetection: ChangeDetectionStrategy.Default
})
export class BiaTableHeaderComponent implements OnChanges {
  @Input() haveFilter = false;
  @Input() showFilter = false;
  @Input() showBtnFilter = false;
  @Input() canAdd = true;
  @Input() canDelete = true;
  @Input() canBack = false;
  @Input() canExportCSV = false;
  @Input() headerTitle: string;
  @Input() selectedElements: any[];
  @Output() create = new EventEmitter();
  @Output() delete = new EventEmitter();
  @Output() openFilter = new EventEmitter();
  @Output() exportCSV = new EventEmitter();

  nbSelectedElements = 0;

  constructor(
    protected location: Location,
    protected router: Router,
    private confirmationService: ConfirmationService,
    private biaDialogService: BiaDialogService
  ) {}

  ngOnChanges() {
    if (this.selectedElements) {
      this.nbSelectedElements = this.selectedElements.length;
    } else {
      this.nbSelectedElements = 0;
    }
  }

  onBack() {
    window.history.length > 1 ? this.location.back() : this.router.navigate(['/']);
  }

  onCreate() {
    this.create.next();
  }

  onDelete() {
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.delete.next();
      }
    };
    this.confirmationService.confirm(confirmation);
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
    if (this.showFilter === true) {
      this.openFilter.emit();
    }
  }
}
