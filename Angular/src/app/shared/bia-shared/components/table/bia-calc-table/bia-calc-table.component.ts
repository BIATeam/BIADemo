import { Component, Output, EventEmitter, SimpleChanges, Input, OnInit, TemplateRef, AfterContentInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
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
export class BiaCalcTableComponent extends BiaTableComponent implements OnInit, AfterContentInit {
  @Input() canAdd = true;
  @Input() canEdit = true;
  @Output() save = new EventEmitter<any>();
  @Input() dictOptionDtos: DictOptionDto[];

  public formId: string;
  public form: UntypedFormGroup;
  public element: any = {};
  public hasChanged = false;
  protected currentRow: HTMLElement;
  protected currentInput: HTMLElement;
  protected sub = new Subscription();
  protected isInComplexInput = false;
  public footerRowData: any;
  public editFooter: boolean = false;

  specificInputTemplate: TemplateRef<any>;

  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(authService, translateService);
  }

  ngAfterContentInit() {
    this.templates.forEach((item) => {
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


  public getOptionDto(key: string) {
    return this.dictOptionDtos.filter((x) => x.key === key)[0]?.value;
  }

  public onElementsChange(changes: SimpleChanges) {
    super.onElementsChange(changes);
    if (changes.elements && this.table) {
      //if (this.elements && this.canAdd === true) {
      this.addFooterEmptyObject();
      //}
    }
  }

  public addFooterEmptyObject() {
    if (this.canAdd === true) {
      this.footerRowData = { id: 0 };
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

  public initEditableRow(rowData: any) {
    if (this.canEdit === true && (!rowData || (rowData &&
      (
        (rowData.id !== 0 && this.table.editingRowKeys[rowData.id] !== true)
        ||
        (rowData.id === 0 && this.editFooter !== true))
    )
    )) {
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
      if (rowData.id === 0) {
        this.editFooter = true;
      }
      else {
        this.editFooter = false;
        this.table.initRowEdit(rowData);
      }
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
    // stop the onFocusout after this code this.currentRow?.focus();
    // because it is launched by the onfocusout of the tr
    if (this.isInComplexInput === false) {
      setTimeout(() => {
        if (this.isInComplexInput !== true &&
          this.getParentComponent(document.activeElement, 'bia-calc-form') === null /*&&
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
      this.currentRow = this.getParentComponent(document.activeElement, 'bia-selectable-row') as HTMLElement;
      if (this.editFooter === true) {
        this.currentInput = this.currentRow?.querySelectorAll('.bia-simple-input')[0] as HTMLElement;
      }
    }
    else {
      if (this.editFooter === true) {
        this.currentInput?.focus();
      } else {
        this.currentRow?.focus();
      }

      this.isInComplexInput = false;
    }
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
