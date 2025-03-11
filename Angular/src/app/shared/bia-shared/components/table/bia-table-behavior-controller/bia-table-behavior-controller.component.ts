import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { CrudConfig } from '../../../feature-templates/crud-items/model/crud-config';

export interface BiaBehaviorIcon {
  tooltip: string;
  disabled: boolean;
  icon: string;
  command: (button: BiaBehaviorIcon) => void;
}

@Component({
  selector: 'bia-table-behavior-controller',
  templateUrl: './bia-table-behavior-controller.component.html',
  styleUrls: ['./bia-table-behavior-controller.component.scss'],
})
export class BiaTableBehaviorControllerComponent<TDto extends { id: number }>
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

  private sub = new Subscription();

  constructor(
    protected readonly translateService: TranslateService,
    protected readonly biaTranslationService: BiaTranslationService
  ) {}

  ngOnInit(): void {
    this.loadActions();

    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.loadActions();
      })
    );
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  private loadActions() {
    this.visibleLayouts = [];
    this.addButtonIfVisible(
      'showCalcMode',
      'useCalcMode',
      'bia.useCalcMode',
      'pi-table'
    );
    this.addButtonIfVisible(
      'showPopup',
      'usePopup',
      'bia.usePopup',
      'pi-clone'
    );
    this.addButtonIfVisible('showSplit', 'useSplit', 'bia.useSplit', 'pi-book');
    this.addFullPageButtonIfVisible();
  }

  private addButtonIfVisible(
    showFlag: keyof typeof this.crudConfiguration.showIcons,
    useFlag: keyof typeof this.crudConfiguration,
    translationKey: string,
    icon: string
  ): void {
    if (this.crudConfiguration.showIcons[showFlag]) {
      const button: BiaBehaviorIcon = {
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

  private addFullPageButtonIfVisible(): void {
    if (this.crudConfiguration.showIcons.showSplit) {
      const isAnyModeActive =
        this.crudConfiguration.useCalcMode ||
        this.crudConfiguration.usePopup ||
        this.crudConfiguration.useSplit;
      const button = {
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

  private setActiveLayout(
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
  }
}
