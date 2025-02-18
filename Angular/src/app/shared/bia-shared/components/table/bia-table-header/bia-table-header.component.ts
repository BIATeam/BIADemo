import { Location } from '@angular/common';
import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  QueryList,
  SimpleChanges,
  TemplateRef,
} from '@angular/core';
import { Router } from '@angular/router';
import { Confirmation, ConfirmationService, PrimeTemplate } from 'primeng/api';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';

@Component({
  selector: 'bia-table-header',
  templateUrl: './bia-table-header.component.html',
  styleUrls: ['./bia-table-header.component.scss'],
  providers: [ConfirmationService],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class BiaTableHeaderComponent implements OnChanges, AfterContentInit {
  @Input() hasFilter = false;
  @Input() showFilter = false;
  @Input() showBtnFilter = false;
  @Input() canAdd = true;
  @Input() canDelete = true;
  @Input() canEdit = true;
  @Input() canImport = false;
  @Input() canBack = false;
  @Input() canExportCSV = false;
  @Input() headerTitle: string;
  @Input() parentDisplayName: string;
  @Input() selectedElements: any[];
  @Input() showTableControllerButton = false;
  @Input() tableControllerVisible = false;
  @Output() create = new EventEmitter<void>();
  @Output() delete = new EventEmitter<void>();
  @Output() openFilter = new EventEmitter<void>();
  @Output() exportCSV = new EventEmitter<void>();
  @Output() fullExportCSV = new EventEmitter<void>();
  @Output() import = new EventEmitter<void>();
  @Output() toggleTableControllerVisibility = new EventEmitter<void>();

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  actionOnSelectedTemplate: TemplateRef<any>;
  actionOnListTemplate: TemplateRef<any>;
  customControlTemplate: TemplateRef<any>;

  nbSelectedElements = 0;
  headerTitleComplete = '';

  constructor(
    protected location: Location,
    protected router: Router,
    protected confirmationService: ConfirmationService,
    protected biaDialogService: BiaDialogService
  ) {}

  ngAfterContentInit() {
    this.templates.forEach(item => {
      switch (item.getType()) {
        case 'actionOnSelected':
          this.actionOnSelectedTemplate = item.template;
          break;
        case 'actionOnList':
          this.actionOnListTemplate = item.template;
          break;
        case 'customControl':
          this.customControlTemplate = item.template;
          break;
      }
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.selectedElements) {
      this.nbSelectedElements = this.selectedElements.length;
    } else {
      this.nbSelectedElements = 0;
    }

    if (changes.parentDisplayName || changes.headerTitle) {
      this.updateHeaderTitle();
    }
  }

  protected updateHeaderTitle() {
    this.headerTitleComplete =
      this.parentDisplayName?.length > 0
        ? `${this.parentDisplayName} - ${this.headerTitle}`
        : this.headerTitle;
  }

  onBack() {
    window.history.length > 1
      ? this.location.back()
      : this.router.navigate(['/']);
  }

  onCreate() {
    this.create.next();
  }

  onDelete() {
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.delete.next();
      },
    };
    this.confirmationService.confirm(confirmation);
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
    if (this.showFilter === true) {
      this.openFilter.emit();
    }
  }

  displayImportButton(): boolean {
    return (
      this.canImport === true &&
      (this.canDelete === true || this.canAdd === true || this.canEdit === true)
    );
  }
}
