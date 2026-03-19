import { AsyncPipe, NgClass, NgStyle, NgTemplateOutlet } from '@angular/common';
import {
  AfterContentInit,
  Component,
  EventEmitter,
  HostListener,
  Input,
  OnInit,
  Output,
  signal,
  SimpleChanges,
  TemplateRef,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
} from '@angular/forms';
import { AuthService, BiaMessageService } from '@bia-team/bia-ng/core';
import { BiaFieldConfig } from '@bia-team/bia-ng/models';
import { DtoState } from '@bia-team/bia-ng/models/enum';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { Skeleton } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { Subscription } from 'rxjs';
import { BiaCalcTableCellComponent } from '../bia-calc-table-cell/bia-calc-table-cell.component';
import { BiaFrozenColumnDirective } from '../bia-frozen-column/bia-frozen-column.directive';
import { BiaTableFilterComponent } from '../bia-table-filter/bia-table-filter.component';
import { BiaTableFooterControllerComponent } from '../bia-table-footer-controller/bia-table-footer-controller.component';
import { BiaTableInputComponent } from '../bia-table-input/bia-table-input.component';
import { BiaTableOutputComponent } from '../bia-table-output/bia-table-output.component';
import { BiaTableComponent } from '../bia-table/bia-table.component';

@Component({
  selector: 'bia-calc-table',
  templateUrl: './bia-calc-table.component.html',
  styleUrls: ['./bia-calc-table.component.scss'],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    PrimeTemplate,
    Tooltip,
    BiaTableFilterComponent,
    NgClass,
    BiaTableInputComponent,
    NgTemplateOutlet,
    BiaTableOutputComponent,
    Skeleton,
    NgStyle,
    BiaTableFooterControllerComponent,
    AsyncPipe,
    TranslateModule,
    BiaFrozenColumnDirective,
    BiaCalcTableCellComponent,
  ],
})
export class BiaCalcTableComponent<TDto extends { id: number | string }>
  extends BiaTableComponent<TDto>
  implements OnInit, AfterContentInit
{
  @Input() canAdd = true;
  @Input() canEdit = true;
  @Output() save = new EventEmitter<any>();
  @Output() isEditing = new EventEmitter<boolean>();

  public formId: string;
  public form: UntypedFormGroup;
  public element: TDto = <TDto>{};
  public hasChanged = false;
  protected currentRow: HTMLTableRowElement;
  protected sub = new Subscription();
  protected complexInputState: 'idle' | 'opening' | 'active' | 'closing' =
    'idle';
  protected isFocusingOut = false;
  public footerRowData: any;
  public editFooter = false;

  // Signals pour la gestion de l'édition
  public editingRowId = signal<number | string | null>(null);
  public editFooterSignal = signal<boolean>(false);

  specificInputTemplate: TemplateRef<any>;

  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(authService, translateService);
  }

  get isInEditing() {
    return this.hasChanged || this.editFooter;
  }

  canDisplayEditInput(field: BiaFieldConfig<TDto>): boolean {
    return (
      field.isEditable === true &&
      (field.isOnlyInitializable === false || field.isOnlyUpdatable === true)
    );
  }

  canDisplayAddInput(field: BiaFieldConfig<TDto>): boolean {
    return field.isEditable === true && field.isOnlyUpdatable === false;
  }

  ngAfterContentInit() {
    this.templates.forEach(item => {
      switch (item.getType()) {
        case 'specificInput':
          this.specificInputTemplate = item.template;
          break;
        case 'specificOutput':
          this.specificOutputTemplate = item.template;
          break;
      }
    });
  }

  ngOnInit() {
    this.initForm();
    this.addFooterEmptyObject();
  }

  firstEditableField(columns: BiaFieldConfig<TDto>[]): number {
    return columns.findIndex(
      col => col.isEditable === true || col.isOnlyInitializable === true
    );
  }

  public getOptionDto(key: string) {
    return this.dictOptionDtos.filter(x => x.key === key)[0]?.value;
  }

  public onElementsChange(changes: SimpleChanges) {
    super.onElementsChange(changes);
    if (changes.elements && this.table) {
      this.addFooterEmptyObject();
    }
  }

  public addFooterEmptyObject() {
    if (this.canAdd === true) {
      this.footerRowData = { id: 0, dtoState: DtoState.Added };
    }
  }

  public addFooterClonedObject(itemTemplate: TDto) {
    if (this.canAdd === true) {
      this.footerRowData = itemTemplate;
    }
  }

  public initForm() {
    throw new Error('initForm not Implemented');
  }

  public isFooter(rowData: any): boolean {
    return rowData?.id === 0 || rowData?.id === '';
  }

  public onChange() {
    this.hasChanged = true;
  }

  public initEditableRowAndFocus(rowData: any, event: MouseEvent) {
    if (this.readOnly) {
      return;
    }

    if (this.canSelectElement && !this.canSelectMultipleElement) {
      this.selectedElements = rowData;
      this.onSelectionChange();
    }
    this.initEditableRow(rowData);
    if (event.currentTarget instanceof HTMLElement) {
      const element = event.currentTarget as HTMLElement;
      setTimeout(() => this.getChildInput(element)?.focus(), 0);
    }
  }

  public initEditableRow(rowData: any, cancelRowEditing = true) {
    if (
      (this.canEdit === true || this.canAdd === true) &&
      (!rowData ||
        (rowData &&
          ((rowData.id !== 0 &&
            rowData.id !== '' &&
            this.table?.editingRowKeys[rowData.id] !== true &&
            rowData.isFixed !== true) ||
            ((rowData.id === 0 || rowData.id === '') &&
              this.editFooter !== true))))
    ) {
      if (this.hasChanged === true) {
        if (!rowData) {
          if (this.form.valid) {
            this.onSubmit();
          } else {
            this.biaMessageService.showWarning(
              this.translateService.instant('biaMsg.invalidForm')
            );
          }
        }
      } else {
        if (cancelRowEditing) {
          this.cancel();
          this.initRowEdit(rowData);
        }
      }
    }
  }

  public initRowEdit(rowData: any) {
    if (rowData) {
      this.complexInputState = 'idle';
      this.element = rowData;
      if (rowData.id === 0 || rowData.id === '') {
        if (this.canAdd === true) {
          this.editFooter = true;
          this.editFooterSignal.set(true);
          this.editingRowId.set(null);
          this.isEditing.emit(true);
        }
      } else {
        this.editFooter = false;
        this.editFooterSignal.set(false);
        if (this.canEdit === true) {
          this.table?.initRowEdit(rowData);
          this.editingRowId.set(rowData.id);
          this.isEditing.emit(true);
        } else {
          this.editingRowId.set(null);
          this.isEditing.emit(false);
        }
      }
      this.form.reset();
      this.form.patchValue({ ...rowData });
    }
  }

  public cancel() {
    this.hasChanged = false;
    this.form.reset();
    if (this.table) {
      this.table.editingRowKeys = {};
    }
    this.editFooter = false;
    this.editFooterSignal.set(false);
    this.editingRowId.set(null);
    this.isEditing.emit(false);
  }

  public escape() {
    this.cancel();
    this.initEditableRow(null);
  }

  public resetEditableRow() {
    this.hasChanged = false;
    this.initEditableRow(null);
  }

  @HostListener('document:keydown.escape') onKeydownHandler() {
    this.escape();
  }

  public onSubmit() {
    throw new Error('onSubmit not Implemented');
  }

  public onFocusout(tr: HTMLTableRowElement) {
    // Avoid multiple onFocuseOut triggers in too short a time
    if (this.isFocusingOut === false) {
      this.isFocusingOut = true;

      // SetTimout is necessary because selecting an option in p-select is not immediately setting back focus to the p-select and document.activeElement isn't updated fast enough
      setTimeout(() => {
        const clickedRowOfActiveElement = this.getParentComponent(
          document.activeElement as Element,
          'bia-selectable-row'
        );

        if (
          // If the complex input is active, don't close the edit mode because the user is probably trying to click on an option in the overlay
          this.complexInputState !== 'active' &&
          this.complexInputState !== 'opening' &&
          // Checking if the clicked element is outside of the current edited row (in case of click on an element of the current edited row, we don't want to close the edit mode)
          clickedRowOfActiveElement !== tr &&
          // If the filter is active in the p-select overlay, the focus switch and stays on the p-select-filter but the overlay is not a children of the row
          !document.activeElement?.className?.includes('p-select')
        ) {
          // If it's a change of row and no change has been made to the previous row, the row editing has already been handle by the initEditableRowAndFocus and should not cancel the edition again
          this.initEditableRow(
            null,
            this.hasChanged || clickedRowOfActiveElement === null
          );
        }
        this.isFocusingOut = false;
      }, 200);
    }
  }

  public onComplexInput(isIn: boolean) {
    // If entering a complex input overlay
    if (isIn) {
      // Saving the row and input and setting the complexInputState as active
      this.complexInputState = 'opening';
      this.currentRow = this.getParentComponent(
        document.activeElement,
        'bia-selectable-row'
      ) as HTMLTableRowElement;
      setTimeout(() => {
        if (this.complexInputState === 'opening') {
          this.complexInputState = 'active';
        }
      }, 200);
    } else {
      // Closing the complexInputState
      if (this.complexInputState === 'active') {
        this.complexInputState = 'closing';
      }
      // Trigger onFocusOut when complexinput is closed because is in overlay and not on tr so onFocusOut is never triggered
      this.onFocusout(this.currentRow);
      setTimeout(() => {
        // Setting complexInputState to idle only if still closing.
        // Reason: If another complex input has been opened when leaving the previous, state will be active and input won't change to idle
        if (this.complexInputState === 'closing') {
          this.complexInputState = 'idle';
        }
      }, 300);
    }
  }

  public getParentComponent(
    el: Element | null,
    parentClassName: string
  ): HTMLElement | null {
    if (el) {
      if (el.classList.contains(parentClassName)) {
        return el as HTMLElement;
      }
      while (el.parentElement) {
        if (el.parentElement.classList.contains(parentClassName)) {
          return el.parentElement;
        } else {
          el = el.parentElement;
        }
      }
    }
    return null;
  }

  public getChildInput(el: Element | null): HTMLElement | null {
    let result: HTMLElement | null = null;
    if (el) {
      const children = el.children;
      for (let i = 0; i < children.length; i++) {
        const tableChild = children[i];
        if (tableChild.tagName === 'INPUT') {
          return tableChild as HTMLElement;
        } else if (tableChild.tagName === 'P-DROPDOWN') {
          return tableChild.children[0].children[0] as HTMLElement;
        } else {
          result = this.getChildInput(tableChild);
        }
        if (result !== null) {
          return result;
        }
      }
    }
    return null;
  }

  public formatDisplayName(objs: any[]): string {
    return objs?.map(x => x.display)?.join(', ');
  }
}
