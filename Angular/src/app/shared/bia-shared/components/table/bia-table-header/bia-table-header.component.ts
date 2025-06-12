import { Location, NgIf, NgTemplateOutlet } from '@angular/common';
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
import { TranslateModule } from '@ngx-translate/core';
import { Confirmation, ConfirmationService, PrimeTemplate } from 'primeng/api';
import { Button, ButtonDirective } from 'primeng/button';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { Divider } from 'primeng/divider';
import { Tooltip } from 'primeng/tooltip';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';

@Component({
  selector: 'bia-table-header',
  templateUrl: './bia-table-header.component.html',
  styleUrls: ['./bia-table-header.component.scss'],
  providers: [ConfirmationService],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    NgIf,
    Button,
    Tooltip,
    ButtonDirective,
    Divider,
    NgTemplateOutlet,
    ConfirmDialog,
    TranslateModule,
  ],
})
export class BiaTableHeaderComponent implements OnChanges, AfterContentInit {
  @Input() canAdd = true;
  @Input() canClone = false;
  @Input() canDelete = true;
  @Input() canEdit = true;
  @Input() canFix = false;
  @Input() canImport = false;
  @Input() canBack = false;
  @Input() canExportCSV = false;
  @Input() headerTitle: string;
  @Input() parentDisplayName: string;
  @Input() selectedElements: any[];
  @Input() showTableControllerButton = false;
  @Input() tableControllerVisible = false;
  @Input() showFixedButtons = false;
  @Output() create = new EventEmitter<void>();
  @Output() delete = new EventEmitter<void>();
  @Output() clone = new EventEmitter<void>();
  @Output() crudItemFixedChanged = new EventEmitter<{
    crudItemId: any;
    fixed: boolean;
  }>();
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
    this.nbSelectedElements = this.selectedElements?.length ?? 0;

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
    if (window.history.length > 1) {
      this.location.back();
    } else {
      this.router.navigate(['/']);
    }
  }

  onCreate() {
    this.create.next();
  }

  onClone() {
    this.clone.next();
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

  displayImportButton(): boolean {
    return (
      this.canImport === true &&
      (this.canDelete === true || this.canAdd === true || this.canEdit === true)
    );
  }

  get isSelectedElementFixed(): boolean {
    return (
      this.selectedElements?.length === 1 &&
      this.selectedElements[0].isFixed === true
    );
  }

  onFixedChanged(fixed: boolean): void {
    this.crudItemFixedChanged.emit({
      crudItemId: this.selectedElements
        ? this.selectedElements[0].id
        : undefined,
      fixed: fixed,
    });
  }

  get isDeleteButtonDisabled(): boolean {
    const selectedElements =
      (this.showFixedButtons === true
        ? this.selectedElements?.filter(e => e.isFixed !== true)
        : this.selectedElements) ?? [];
    return selectedElements.length === 0;
  }
}
