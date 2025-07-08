import { NgFor, NgIf } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Popover } from 'primeng/popover';
import { Tooltip } from 'primeng/tooltip';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { CrudConfig } from '../../../feature-templates/crud-items/model/crud-config';
import { CrudItemService } from '../../../feature-templates/crud-items/services/crud-item.service';
import { BaseDto } from '../../../model/dto/base-dto';

export interface BiaBehaviorIcon {
  name: 'CalcMode' | 'Popup' | 'Split' | 'FullPage';
  tooltip: string;
  disabled: boolean;
  icon: string;
  command: (button: BiaBehaviorIcon) => void;
}

@Component({
  selector: 'bia-table-behavior-controller',
  templateUrl: './bia-table-behavior-controller.component.html',
  styleUrls: ['./bia-table-behavior-controller.component.scss'],
  imports: [Tooltip, NgIf, Popover, NgFor, TranslateModule],
})
export class BiaTableBehaviorControllerComponent<TDto extends BaseDto>
  implements OnInit, OnDestroy
{
  selectedLayout?: BiaBehaviorIcon;
  visibleLayouts: BiaBehaviorIcon[];

  @Input() crudConfiguration: CrudConfig<TDto>;

  @Output() useCalcModeChanged = new EventEmitter<boolean>();
  @Output() usePopupChanged = new EventEmitter<boolean>();
  @Output() useSplitChanged = new EventEmitter<boolean>();
  @Output() useSignalRChanged = new EventEmitter<boolean>();
  @Output() useViewChanged = new EventEmitter<boolean>();
  @Output() useCompactModeChanged = new EventEmitter<boolean>();
  @Output() useVirtualScrollChanged = new EventEmitter<boolean>();
  @Output() useResizableColumnChanged = new EventEmitter<boolean>();

  @ViewChild(Popover) popover: Popover;

  protected sub = new Subscription();

  constructor(
    protected readonly translateService: TranslateService,
    protected readonly biaTranslationService: BiaTranslationService,
    protected readonly crudItemService: CrudItemService<TDto>
  ) {}

  ngOnInit(): void {
    this.loadActions();

    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.loadActions();
      })
    );

    if (this.crudItemService) {
      this.sub.add(
        this.crudItemService.configChanged$.subscribe(() =>
          this.updateSelectedLayout()
        )
      );
    }
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  protected updateSelectedLayout(): void {
    if (this.visibleLayouts) {
      this.selectedLayout = this.visibleLayouts.find(button => {
        if (this.crudConfiguration.useCalcMode) {
          return button.name === 'CalcMode';
        }
        if (this.crudConfiguration.usePopup) {
          return button.name === 'Popup';
        }
        if (this.crudConfiguration.useSplit) {
          return button.name === 'Split';
        }
        return button.name === 'FullPage';
      });

      this.visibleLayouts.forEach(layout => {
        layout.disabled = true;
      });
      if (this.selectedLayout) {
        this.selectedLayout.disabled = false;
      }
    }
  }

  protected loadActions() {
    this.visibleLayouts = [];
    this.addButtonIfVisible('CalcMode', 'pi-table');
    this.addButtonIfVisible('Popup', 'pi-clone');
    this.addButtonIfVisible('Split', 'pi-stop');
    this.addFullPageButtonIfVisible();
  }

  protected addButtonIfVisible(
    name: 'CalcMode' | 'Popup' | 'Split',
    icon: string
  ): void {
    const showFlag: keyof typeof this.crudConfiguration.showIcons = ('show' +
      name) as keyof typeof this.crudConfiguration.showIcons;
    const useFlag: keyof typeof this.crudConfiguration = ('use' +
      name) as keyof typeof this.crudConfiguration;
    const translationKey: string = 'bia.use' + name;
    if (this.crudConfiguration.showIcons[showFlag]) {
      const button: BiaBehaviorIcon = {
        name,
        tooltip: this.translateService.instant(translationKey),
        command: (button: BiaBehaviorIcon) =>
          this.setActiveLayout(useFlag, button),
        disabled: !this.crudConfiguration[useFlag],
        icon,
      };
      this.visibleLayouts.push(button);
      if (this.crudConfiguration[useFlag]) {
        this.selectedLayout = button;
      }
    }
  }

  protected addFullPageButtonIfVisible(): void {
    if (
      this.crudConfiguration.showIcons.showSplit ||
      this.crudConfiguration.showIcons.showCalcMode ||
      this.crudConfiguration.showIcons.showPopup
    ) {
      const isAnyModeActive =
        this.crudConfiguration.useCalcMode ||
        this.crudConfiguration.usePopup ||
        this.crudConfiguration.useSplit;
      const button = {
        name: 'FullPage' as const,
        tooltip: this.translateService.instant('bia.useFullPage'),
        command: (button: BiaBehaviorIcon) =>
          this.setActiveLayout(null, button),
        disabled: isAnyModeActive,
        icon: 'pi-stop',
      };
      this.visibleLayouts.push(button);
      if (!isAnyModeActive) {
        this.selectedLayout = button;
      }
    }
  }

  protected setActiveLayout(
    activeFlag: keyof typeof this.crudConfiguration | null,
    button: BiaBehaviorIcon
  ): void {
    this.useCalcModeChanged.emit(false);
    this.usePopupChanged.emit(false);
    this.useSplitChanged.emit(false);

    if (activeFlag !== null) {
      (this as any)[`${activeFlag}Changed`].emit(true);
    }
    this.selectedLayout = button;

    this.visibleLayouts.forEach(layout => {
      layout.disabled = true;
    });
    button.disabled = false;
    this.popover.hide();
  }
}
