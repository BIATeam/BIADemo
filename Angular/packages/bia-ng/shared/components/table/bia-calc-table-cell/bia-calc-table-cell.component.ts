import { NgTemplateOutlet } from '@angular/common';
import {
  AfterContentInit,
  Component,
  ContentChildren,
  Input,
  QueryList,
  TemplateRef,
  WritableSignal,
} from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { BiaFieldConfig } from 'packages/bia-ng/models/public-api';
import { PrimeTemplate } from 'primeng/api';
import { MultiSelect } from 'primeng/multiselect';
import { BiaTableInputComponent } from '../bia-table-input/bia-table-input.component';
import { BiaTableOutputComponent } from '../bia-table-output/bia-table-output.component';

@Component({
  selector: 'bia-calc-table-cell',
  templateUrl: './bia-calc-table-cell.component.html',
  styleUrls: ['./bia-calc-table-cell.component.scss'],
  imports: [
    BiaTableInputComponent,
    BiaTableOutputComponent,
    NgTemplateOutlet,
    PrimeTemplate,
  ],
})
export class BiaCalcTableCellComponent<
  TDto extends { id: number | string },
> implements AfterContentInit {
  @Input() col: BiaFieldConfig<TDto>;
  @Input() rowData: TDto;
  @Input() form: UntypedFormGroup;
  @Input() dictOptionDtos: any[];
  @Input() editingRowId: WritableSignal<number | string | null>;
  @Input() editFooterSignal: WritableSignal<boolean>;
  @Input() onChange: () => void;
  @Input() onComplexInput: (event: boolean) => void;
  @Input() onPanelHide: (event: MultiSelect) => void;

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  specificInputTemplate: TemplateRef<any>;
  specificOutputTemplate: TemplateRef<any>;

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

  canDisplayEditInput(field: BiaFieldConfig<TDto>): boolean {
    return (
      field.isEditable === true &&
      (field.isOnlyInitializable === false || field.isOnlyUpdatable === true)
    );
  }

  isFooter(rowData: any): boolean {
    return rowData?.id === 0 || rowData?.id === '';
  }

  getCellData(rowData: TDto, field: BiaFieldConfig<TDto>): any {
    if (field.field && rowData) {
      return (rowData as any)[field.field];
    }
    return null;
  }
}
