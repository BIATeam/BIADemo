import { AsyncPipe, NgClass, NgStyle, NgTemplateOutlet } from '@angular/common';
import {
  AfterContentInit,
  Component,
  EventEmitter,
  HostListener,
  Input,
  OnInit,
  Output,
  SimpleChanges,
  TemplateRef,
} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
} from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import {
  AuthService,
  BiaMessageService,
} from 'packages/bia-ng/core/public-api';
import { DtoState } from 'packages/bia-ng/models/enum/public-api';
import { BiaFieldConfig } from 'packages/bia-ng/models/public-api';
import { PrimeTemplate } from 'primeng/api';
import { Skeleton } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { Subscription } from 'rxjs';
import { BiaFrozenColumnDirective } from '../bia-frozen-column/bia-frozen-column.directive';
import { BiaTableFilterComponent } from '../bia-table-filter/bia-table-filter.component';
import { BiaTableFooterControllerComponent } from '../bia-table-footer-controller/bia-table-footer-controller.component';
import { BiaTableInputComponent } from '../bia-table-input/bia-table-input.component';
import { BiaTableOutputComponent } from '../bia-table-output/bia-table-output.component';
import { BiaTableComponent } from '../bia-table/bia-table.component';
import { DictOptionDto } from '../bia-table/dict-option-dto';

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
  ],
})
export class BiaCalcTableComponent<TDto extends { id: number | string }>
  extends BiaTableComponent<TDto>
  implements OnInit, AfterContentInit
{
  @Input() canAdd = true;
  @Input() canEdit = true;
  @Input() dictOptionDtos: DictOptionDto[];
  @Output() save = new EventEmitter<any>();
  @Output() isEditing = new EventEmitter<boolean>();

  public formId: string;
  public form: UntypedFormGroup;
  public element: TDto = <TDto>{};
  public hasChanged = false;
  protected currentRow: HTMLElement;
  protected currentInput: HTMLElement;
  protected sub = new Subscription();
  protected isInComplexInput = false;
  public footerRowData: any;
  public editFooter = false;

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
    return (
      field.isEditable === true &&
      (field.isOnlyUpdatable === false || field.isOnlyInitializable === true)
    );
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

  public isFooter(element: any) {
    return element.id === 0 || element.id === '';
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

  public initEditableRow(rowData: any) {
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
        this.cancel();
        this.initRowEdit(rowData);
      }
    }
  }

  public initRowEdit(rowData: any) {
    if (rowData) {
      this.element = rowData;
      if (rowData.id === 0 || rowData.id === '') {
        if (this.canAdd === true) {
          this.editFooter = true;
          this.isEditing.emit(true);
        }
      } else {
        this.editFooter = false;
        if (this.canEdit === true) {
          this.table?.initRowEdit(rowData);
          this.isEditing.emit(true);
        } else {
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

  @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
    this.escape();
  }

  public onSubmit() {
    throw new Error('onSubmit not Implemented');
  }

  public onFocusout() {
    // stop the onFocusout after this code this.currentRow?.focus();
    // because it is launched by the onfocusout of the tr
    if (this.isInComplexInput === false) {
      setTimeout(() => {
        if (
          this.isInComplexInput !== true &&
          this.getParentComponent(document.activeElement, 'bia-calc-form') ===
            null &&
          !document.activeElement?.className?.includes('p-select') /*&&
          this.getParentComponent(document.activeElement, 'p-datepicker') === null*/
        ) {
          this.initEditableRow(null);
        }
      }, 200);
    }
  }

  public onComplexInput(isIn: boolean) {
    if (isIn) {
      this.isInComplexInput = true;
      this.currentRow = this.getParentComponent(
        document.activeElement,
        'bia-selectable-row'
      ) as HTMLElement;
      this.currentInput = document.activeElement as HTMLElement;
    } else {
      if (
        // If selected element parent is the same as complex input parent, don't change focus. If not, focus complex input
        (this.getParentComponent(
          document.activeElement,
          'bia-selectable-row'
        ) as HTMLElement) !== this.currentRow
      ) {
        this.currentInput?.focus();
      }
      this.isInComplexInput = false;
    }
  }

  public getParentComponent(
    el: Element | null,
    parentClassName: string
  ): HTMLElement | null {
    if (el) {
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
