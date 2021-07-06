import { Component, Output, EventEmitter, SimpleChanges, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { DictOptionDto } from '../bia-table/dict-option-dto';

@Component({
  selector: 'bia-calc-table',
  templateUrl: './bia-calc-table.component.html',
  styleUrls: ['../bia-table/bia-table.component.scss']
})
export class BiaCalcTableComponent extends BiaTableComponent implements OnInit {
  @Input() canAdd = true;
  @Output() save = new EventEmitter<any>();
  @Input() dictOptionDtos: DictOptionDto[];

  public formId: string;
  public form: FormGroup;
  public element: any = {};
  public hasChanged = false;
  public specificInputs: string[] = [];
  protected mandatoryFields: string[] = [];

  protected sub = new Subscription();

  protected currentRow: HTMLElement;
  protected isInMultiSelect = false;

  constructor(
    public formBuilder: FormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(authService);
    this.initForm();
  }

  ngOnInit() {
    this.fillMandatoryFields();
  }

  protected fillMandatoryFields() {
    Object.keys(this.form.controls).forEach(key => {
      if (this.form.controls[key]?.validator?.name === 'required') {
        this.mandatoryFields.push(key);
      }
    });
  }

  public isRequired(field: string): boolean {
    return this.mandatoryFields.includes(field);
  }

  public getOptionDto(key: string) {
    return this.dictOptionDtos.filter((x) => x.key === key)[0]?.value;
  }

  public onElementsChange(changes: SimpleChanges) {
    super.onElementsChange(changes);
    if (changes.elements && this.table) {
      if (this.elements && this.canAdd === true) {
        this.addFooterEmptyObject();
      }
    }
  }

  public addFooterEmptyObject() {
    if (this.elements.filter(el => el.id === 0).length === 0) {
      this.elements = [...this.elements, { id: 0 }];
    }
  }

  public initForm() {
    throw new Error('initForm not Implemented');
  }

  public isFooter(element: any) {
    return element.id === 0;
  }

  public onChange() {
    this.hasChanged = true;
  }

  public isSpecificInput(field: string) {
    return this.specificInputs && this.specificInputs.some((x) => field === x);
  }

  public initEditableRow(rowData: any) {
    if (this.canEdit === true && (!rowData || (rowData && this.table.editingRowKeys[rowData.id] !== true))) {
      if (this.hasChanged === true) {
        if (this.form.valid) {
          this.onSave();
          this.cancel();
          this.initRowEdit(rowData);
        } else {
          this.biaMessageService.showWarning(this.translateService.instant('biaMsg.invalidForm'));
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
      this.table.initRowEdit(rowData);
      this.form.reset();
      this.form.patchValue({ ...rowData });
    }
  }

  public onSave() {
    if (this.hasChanged === true) {
      this.onSubmit();
      this.hasChanged = false;
    }
  }

  public cancel() {
    this.hasChanged = false;
    this.form.reset();
    this.table.editingRowKeys = {};
  }

  public onSubmit() {
    throw new Error('onSubmit not Implemented');
  }

  public onFocusout() {
    setTimeout(() => {
      if (this.isInMultiSelect !== true && this.getParentComponent(document.activeElement, 'bia-calc-form') === null) {
        this.initEditableRow(null);
      }
    }, 200);
  }

  public onShowCalendar() {
    this.currentRow = this.getParentComponent(document.activeElement, 'ui-selectable-row') as HTMLElement;
  }

  public onBlurCalendar() {
    this.currentRow?.focus();
  }

  public onPanelShowMultiSelect() {
    this.isInMultiSelect = true;
  }

  public onPanelHideMultiSelect() {
    this.isInMultiSelect = false;
    this.onFocusout();
  }

  public getParentComponent(el: Element | null, parentClassName: string): HTMLElement | null {
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

  public formatDisplayName(objs: any[]): string {
    return objs?.map(x => x.display)?.join(', ');
  }
}
