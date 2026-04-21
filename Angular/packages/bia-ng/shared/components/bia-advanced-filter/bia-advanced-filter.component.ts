import { NgTemplateOutlet } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  TemplateRef,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { BiaTranslationService } from 'packages/bia-ng/core/public-api';
import { PropType } from 'packages/bia-ng/models/enum/public-api';
import {
  BiaAdvancedFilterConfig,
  BiaAdvancedFilterFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
  OptionDto,
} from 'packages/bia-ng/models/public-api';
import { ButtonDirective } from 'primeng/button';
import { DatePicker } from 'primeng/datepicker';
import { FloatLabel } from 'primeng/floatlabel';
import { InputNumber } from 'primeng/inputnumber';
import { InputText } from 'primeng/inputtext';
import { MultiSelect } from 'primeng/multiselect';
import { Select } from 'primeng/select';
import { Tooltip } from 'primeng/tooltip';
import { Subscription } from 'rxjs';
import { BiaFieldHelperService } from '../form/bia-field-base/bia-field-helper.service';
import { DictOptionDto } from '../table/bia-table/dict-option-dto';

export interface BiaAdvancedFilterResolvedField<TAdvancedFilter> {
  fieldConfig: BiaAdvancedFilterFieldConfig<TAdvancedFilter>;
  type: PropType;
  controlName: string;
  numberFormat?: BiaFieldNumberFormat;
  dateFormat?: BiaFieldDateFormat;
  resolvedOptions: OptionDto[];
}

@Component({
  selector: 'bia-advanced-filter',
  templateUrl: './bia-advanced-filter.component.html',
  imports: [
    NgTemplateOutlet,
    ButtonDirective,
    Tooltip,
    FormsModule,
    ReactiveFormsModule,
    Select,
    MultiSelect,
    InputNumber,
    DatePicker,
    InputText,
    TranslateModule,
    FloatLabel,
  ],
})
export class BiaAdvancedFilterComponent<TAdvancedFilter>
  implements OnInit, OnChanges, OnDestroy
{
  @ViewChild('template', { static: true }) template: TemplateRef<HTMLElement>;

  @Input() filterConfig: BiaAdvancedFilterConfig<TAdvancedFilter>;
  @Input() dictOptionDtos: DictOptionDto[] = [];
  @Input() advancedFilter: TAdvancedFilter | null = null;
  @Input() hidden = false;
  @Input() specificInputTemplate: TemplateRef<any>;

  @Output() closeFilter = new EventEmitter<void>();
  @Output() filter = new EventEmitter<TAdvancedFilter>();

  readonly propType = PropType;

  form: FormGroup;
  submittingForm = false;
  resolvedFields: BiaAdvancedFilterResolvedField<TAdvancedFilter>[] = [];

  private sub = new Subscription();

  constructor(
    private fb: FormBuilder,
    private viewContainerRef: ViewContainerRef,
    private biaTranslationService: BiaTranslationService
  ) {}

  ngOnInit() {
    this.viewContainerRef.createEmbeddedView(this.template);
    this.buildResolvedFields();
    this.buildForm();
    this.initLocaleSubscriptions();
    if (this.advancedFilter) {
      this.patchForm(this.advancedFilter);
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['filterConfig'] && !changes['filterConfig'].firstChange) {
      this.buildResolvedFields();
      this.buildForm();
      this.initLocaleSubscriptions();
    }
    if (changes['dictOptionDtos'] && this.resolvedFields?.length) {
      for (const f of this.resolvedFields) {
        if (!f.fieldConfig.options?.length && !f.fieldConfig.options$) {
          f.resolvedOptions =
            this.dictOptionDtos?.find(x => x.key === f.fieldConfig.field)
              ?.value ?? [];
        }
      }
    }
    if (changes['advancedFilter'] && this.form) {
      this.patchForm(this.advancedFilter ?? ({} as TAdvancedFilter));
    }
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  getOptions(f: BiaAdvancedFilterResolvedField<TAdvancedFilter>) {
    return f.resolvedOptions;
  }

  getDateView(dateFormat: string) {
    const hasY = /[yY]/.test(dateFormat);
    const hasM = /[mM]/.test(dateFormat);
    const hasD = /[dD]/.test(dateFormat);
    if (hasY && !hasM && !hasD) return 'year';
    if (hasM && !hasD) return 'month';
    return 'date';
  }

  onClose() {
    this.closeFilter.emit();
  }
  onReset() {
    this.form.reset();
  }

  onFilter() {
    if (this.form.valid) {
      this.submittingForm = true;
      this.filter.emit(this.form.value as TAdvancedFilter);
      setTimeout(() => (this.submittingForm = false), 2000);
    }
  }

  private buildResolvedFields() {
    this.resolvedFields = (this.filterConfig?.fields ?? []).map(fieldConfig => {
      const resolved: BiaAdvancedFilterResolvedField<TAdvancedFilter> = {
        fieldConfig,
        type: fieldConfig.type,
        controlName: fieldConfig.field,
        resolvedOptions: fieldConfig.options ?? [],
      };

      if (fieldConfig.options$) {
        this.sub.add(
          fieldConfig.options$.subscribe((opts: OptionDto[]) => {
            resolved.resolvedOptions = opts;
          })
        );
      } else if (!fieldConfig.options?.length) {
        resolved.resolvedOptions =
          this.dictOptionDtos?.find(x => x.key === fieldConfig.field)?.value ??
          [];
      }

      if (fieldConfig.numberFormat) {
        resolved.numberFormat = fieldConfig.numberFormat;
      } else if (fieldConfig.type === PropType.Number) {
        resolved.numberFormat = new BiaFieldNumberFormat();
      }

      if (fieldConfig.dateFormat) {
        resolved.dateFormat = fieldConfig.dateFormat;
      } else if (
        fieldConfig.type === PropType.Date ||
        fieldConfig.type === PropType.DateTime
      ) {
        resolved.dateFormat = new BiaFieldDateFormat();
      }

      return resolved;
    });
  }

  private buildForm() {
    const controls: Record<string, any> = {};
    for (const f of this.resolvedFields) {
      controls[f.controlName] = [null];
    }
    this.form = this.fb.group(controls);
  }

  private initLocaleSubscriptions() {
    this.sub.unsubscribe();
    this.sub = new Subscription();

    const numberFields = this.resolvedFields.filter(
      f => f.type === PropType.Number
    );
    if (numberFields.length) {
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(culture => {
          if (culture) {
            for (const f of numberFields) {
              f.numberFormat = Object.assign(
                new BiaFieldNumberFormat(),
                f.fieldConfig.numberFormat ?? {},
                { autoLocale: culture }
              );
            }
          }
        })
      );
    }

    const dateFields = this.resolvedFields.filter(
      f => f.type === PropType.Date || f.type === PropType.DateTime
    );
    if (dateFields.length) {
      this.sub.add(
        this.biaTranslationService.currentCultureDateFormat$.subscribe(
          dateFormat => {
            if (dateFormat) {
              for (const f of dateFields) {
                if (!f.fieldConfig.dateFormat) {
                  const dummy = {
                    displayFormat: new BiaFieldDateFormat(),
                    clone: () => dummy,
                  } as any;
                  BiaFieldHelperService.setDateFormat(dummy, dateFormat);
                  f.dateFormat = dummy.displayFormat;
                } else {
                  f.dateFormat = f.fieldConfig.dateFormat;
                }
              }
            }
          }
        )
      );
    }
  }

  private patchForm(value: TAdvancedFilter) {
    const v = value as Record<string, any>;
    const patch: Record<string, any> = {};
    for (const f of this.resolvedFields) {
      patch[f.controlName] = v[f.controlName] ?? null;
    }
    this.form.patchValue(patch);
  }
}
