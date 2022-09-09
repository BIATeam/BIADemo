import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  Input,
  QueryList,
  TemplateRef
} from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PrimeTemplate } from 'primeng/api';
import { PrimeTableColumn} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { DictOptionDto } from 'src/app/shared/bia-shared/components/table/bia-table/dict-option-dto';

@Component({
  selector: 'bia-field',
  templateUrl: './bia-field.component.html',
  styleUrls: ['./bia-field.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class BiaFieldComponent implements AfterContentInit {
  @Input() field: PrimeTableColumn;
  @Input() form: FormGroup;
  @Input() dictOptionDtos: DictOptionDto[];

  @ContentChildren(PrimeTemplate) templates: QueryList<any>;
  // specificInputTemplate: TemplateRef<any>;
  specificInputTemplate: TemplateRef<any>;

  constructor(
      public formBuilder: FormBuilder,
      // protected authService: AuthService
    ) {
    
  }

  ngAfterContentInit() {
    this.templates.forEach((item) => {
        switch(item.getType()) {
          /*case 'specificInput':
            this.specificInputTemplate = item.template;
          break;*/
          case 'specificInput':
            this.specificInputTemplate = item.template;
          break;
        }
    });
  }

  public getOptionDto(key: string) {
    return this.dictOptionDtos.filter((x) => x.key === key)[0]?.value;
  }
  
}
