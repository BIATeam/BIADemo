import { Location, NgIf, NgTemplateOutlet } from '@angular/common';
import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  Output,
  QueryList,
  SimpleChanges,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { BiaDialogService } from 'packages/bia-ng/core/public-api';
import {
  Confirmation,
  ConfirmationService,
  MenuItem,
  PrimeTemplate,
} from 'primeng/api';
import { Button, ButtonDirective } from 'primeng/button';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { Divider } from 'primeng/divider';
import { MenuModule } from 'primeng/menu';
import { Tooltip } from 'primeng/tooltip';
import { ThrottleEventDirective } from '../../../directives/throttle-click.directive';

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
    MenuModule,
    ThrottleEventDirective,
  ],
})
export class BiaTableHeaderComponent
  implements OnChanges, AfterContentInit, OnDestroy
{
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
  @Input() showHistoricalButton = false;
  @Input() selectionActionsMenuItems?: MenuItem[];
  @Input() listActionsMenuItems?: MenuItem[];
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

  selectionActions: ActionMenuItems = {
    compact: false,
    actionList: [],
    hasVisibleAction: false,
  };
  listActions: ActionMenuItems = {
    compact: false,
    actionList: [],
    hasVisibleAction: false,
  };

  @ViewChild('selectionActionsDiv', { static: false })
  selectionActionsDiv: ElementRef<HTMLDivElement>;
  @ViewChild('listActionsDiv', { static: false })
  listActionsDiv: ElementRef<HTMLDivElement>;
  @ViewChild('headerDiv', { static: false })
  headerDiv: ElementRef<HTMLDivElement>;

  protected parentContainerResizeObserver!: ResizeObserver;

  constructor(
    protected location: Location,
    protected router: Router,
    protected confirmationService: ConfirmationService,
    protected biaDialogService: BiaDialogService,
    protected activatedRoute: ActivatedRoute,
    protected transationService: TranslateService
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
    this.processSelectionActions();
    this.processListActions();
    setTimeout(() => {
      this.initParentContainerResizeObserver();
    }, 500);
  }

  ngOnChanges(changes: SimpleChanges) {
    this.nbSelectedElements = this.selectedElements?.length ?? 0;

    if (changes.parentDisplayName || changes.headerTitle) {
      this.updateHeaderTitle();
    }

    this.processSelectionActions();
    this.processListActions();
  }

  ngOnDestroy(): void {
    if (this.parentContainerResizeObserver) {
      this.parentContainerResizeObserver.disconnect();
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

  onShowHistorical() {
    if (this.selectedElements.length === 1) {
      this.router.navigate([this.selectedElements[0].id, 'historical'], {
        relativeTo: this.activatedRoute,
      });
    }
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

  protected processSelectionActions() {
    this.selectionActions.actionList = [
      {
        label: this.transationService.instant('bia.delete'),
        icon: 'pi pi-trash',
        command: () => this.onDelete(),
        disabled: this.isDeleteButtonDisabled,
        visible: this.canDelete,
      },
      {
        label: this.transationService.instant('bia.historical'),
        icon: 'pi pi-history',
        command: () => this.onShowHistorical(),
        disabled: this.nbSelectedElements !== 1,
        visible: this.showHistoricalButton,
      },
      {
        label: this.transationService.instant('bia.clone'),
        icon: 'pi pi-copy',
        styleClass: 'p-button-outlined',
        command: () => this.onClone(),
        disabled: this.nbSelectedElements !== 1,
        visible: this.canClone,
      },
      { separator: true, visible: this.showFixedButtons && this.canFix },
      {
        label: this.transationService.instant('bia.fix'),
        icon: 'pi pi-lock',
        command: () => this.onFixedChanged(true),
        disabled: this.nbSelectedElements !== 1 || this.isSelectedElementFixed,
        visible: this.showFixedButtons && this.canFix,
      },
      {
        label: this.transationService.instant('bia.unfix'),
        icon: 'pi pi-unlock',
        styleClass: 'p-button-outlined',
        command: () => this.onFixedChanged(false),
        disabled: this.nbSelectedElements !== 1 || !this.isSelectedElementFixed,
        visible: this.showFixedButtons && this.canFix,
      },
      ...(this.selectionActionsMenuItems ?? []),
    ];
    this.processActions(this.selectionActions);
  }

  protected processListActions() {
    this.listActions.actionList = [
      {
        label: this.transationService.instant('bia.add'),
        icon: 'pi pi-plus',
        command: () => {
          this.onCreate();
        },
        visible: this.canAdd,
      },
      { separator: true },
      {
        styleClass: 'p-button-outlined',
        icon: 'pi pi-download',
        label: 'CSV',
        command: () => this.exportCSV.emit(),
        visible: this.canExportCSV,
      },
      { separator: true },
      {
        icon: 'pi pi-plus',
        styleClass: 'p-button-outlined',
        label: this.transationService.instant('bia.import'),
        command: () => this.import.next(),
        visible: this.displayImportButton(),
      },
      {
        styleClass: 'p-button-outlined',
        icon: 'pi pi-download',
        label: this.transationService.instant('bia.export'),
        command: () => this.fullExportCSV.emit(),
        visible: this.displayImportButton(),
      },
      { separator: true },
      {
        styleClass: 'p-button-outlined',
        label: this.transationService.instant('bia.back'),
        command: () => this.onBack(),
        visible: this.canBack,
      },
      ...(this.listActionsMenuItems ?? []),
    ];
    this.processActions(this.listActions);
  }

  protected processActions(actionMenuItems: ActionMenuItems) {
    actionMenuItems.hasVisibleAction = false;

    let lastSeparatorIndex = -1;
    actionMenuItems.actionList.forEach((action, index) => {
      if (!action.separator && action.visible) {
        actionMenuItems.hasVisibleAction = true;
      }

      if (action.separator) {
        const hasVisibleBefore = actionMenuItems.actionList
          .slice(lastSeparatorIndex + 1, index)
          .some(a => !a.separator && a.visible);
        const nextSeparatorIndex = actionMenuItems.actionList.findIndex(
          (a, i) => a.separator && i > index
        );
        const endIndex =
          nextSeparatorIndex === -1
            ? actionMenuItems.actionList.length
            : nextSeparatorIndex;
        const hasVisibleAfter = actionMenuItems.actionList
          .slice(index + 1, endIndex)
          .some(a => !a.separator && a.visible);
        action.visible = hasVisibleBefore && hasVisibleAfter;

        lastSeparatorIndex = index;
      }
    });
  }

  protected initParentContainerResizeObserver() {
    const parentContainer = this.headerDiv.nativeElement;
    if (!parentContainer || parentContainer.children.length === 1) {
      return;
    }

    this.parentContainerResizeObserver = new ResizeObserver(() => {
      this.onParentContainerResized(
        this.selectionActionsDiv.nativeElement,
        this.selectionActions
      );
      this.onParentContainerResized(
        this.listActionsDiv.nativeElement,
        this.listActions
      );
    });
    this.parentContainerResizeObserver.observe(parentContainer);
  }

  protected onParentContainerResized(
    container: Element,
    actionMenuItems: ActionMenuItems
  ) {
    const offset = 20;
    const minWidth = 140;

    if (!actionMenuItems.containerWidth || !actionMenuItems.compact) {
      actionMenuItems.containerWidth = container.getBoundingClientRect().width;
    }

    if (actionMenuItems.containerWidth > minWidth) {
      if (container.nextElementSibling) {
        actionMenuItems.compact =
          container.getBoundingClientRect().left +
            actionMenuItems.containerWidth +
            offset >=
          container.nextElementSibling.getBoundingClientRect().left;
        return;
      }

      if (container.previousElementSibling) {
        actionMenuItems.compact =
          container.previousElementSibling.getBoundingClientRect().right >=
          container.getBoundingClientRect().right -
            actionMenuItems.containerWidth -
            offset;
      }
    }
  }

  hasVisibleMenuItems(actions: ActionMenuItems): boolean {
    if (!actions.actionList) return false;
    return actions.actionList.some(item => item.visible !== false);
  }
}

export interface ActionMenuItems {
  containerWidth?: number;
  compact: boolean;
  actionList: MenuItem[];
  hasVisibleAction: boolean;
}
