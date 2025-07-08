import { TeamTypeId } from 'src/app/shared/constants';
import { BiaFieldsConfig } from '../../../model/bia-field-config';
import { BiaFormLayoutConfig } from '../../../model/bia-form-layout-config';
import { BiaTableState } from '../../../model/bia-table-state';

export enum FormReadOnlyMode {
  off,
  clickToEdit,
  on,
}

export interface ShowIconsConfig {
  showCalcMode: boolean;
  showPopup: boolean;
  showSplit: boolean;
  showView: boolean;
  showSignalR: boolean;
  showCompactMode: boolean;
  showVirtualScroll: boolean;
  showResizableColumn: boolean;
}

export class CrudConfig<TDto extends { id: number }> {
  featureName: string;
  storeKey: string;
  useCalcMode: boolean;
  useSignalR: boolean;
  useView: boolean;
  tableStateKey: string;
  useViewTeamWithTypeId: TeamTypeId | null;
  usePopup: boolean;
  useSplit: boolean;
  useOfflineMode: boolean;
  fieldsConfig: BiaFieldsConfig<TDto>;
  formLayoutConfig: BiaFormLayoutConfig<TDto> | undefined;
  defaultViewPref: BiaTableState;
  optionFilter: any;
  useImport: boolean;
  useCompactMode?: boolean;
  useVirtualScroll = false;
  useResizableColumn = false;
  importMode?: {
    useInsert: boolean;
    useUpdate: boolean;
    useDelete: boolean;
  };
  showIcons: ShowIconsConfig = {
    showCalcMode: false,
    showPopup: false,
    showSplit: false,
    showView: false,
    showSignalR: false,
    showCompactMode: false,
    showVirtualScroll: false,
    showResizableColumn: false,
  };
  formEditReadOnlyMode: FormReadOnlyMode;
  isFixable: boolean;
  isCloneable: boolean;
  hasReadView: boolean;

  constructor({
    featureName,
    fieldsConfig,
    formLayoutConfig = undefined,
    formEditReadOnlyMode = FormReadOnlyMode.off,
    storeKey = 'feature-' + featureName,
    useCalcMode = false,
    useSignalR = false,
    useView = false,
    tableStateKey = featureName + 'Grid',
    useViewTeamWithTypeId = null,
    usePopup = true,
    useSplit = false,
    useOfflineMode = false,
    optionFilter = undefined,
    importMode,
    useCompactMode = false,
    useVirtualScroll = false,
    useResizableColumn = false,
    showIcons,
    isFixable = false,
    isCloneable = false,
    hasReadView = false,
  }: {
    featureName: string;
    fieldsConfig: BiaFieldsConfig<TDto>;
    formLayoutConfig?: BiaFormLayoutConfig<TDto> | undefined;
    formEditReadOnlyMode?: FormReadOnlyMode;
    storeKey?: string;
    useCalcMode?: boolean;
    useSignalR?: boolean;
    useView?: boolean;
    tableStateKey?: string;
    useViewTeamWithTypeId?: TeamTypeId | null;
    usePopup?: boolean;
    useSplit?: boolean;
    useOfflineMode?: boolean;
    optionFilter?: any;
    importMode?: {
      useInsert: boolean;
      useUpdate: boolean;
      useDelete: boolean;
    };
    useCompactMode?: boolean;
    useVirtualScroll?: boolean;
    useResizableColumn?: boolean;
    showIcons?: Partial<ShowIconsConfig>;
    isFixable?: boolean;
    isCloneable?: boolean;
    hasReadView?: boolean;
  }) {
    this.featureName = featureName;
    this.fieldsConfig = fieldsConfig;
    this.formLayoutConfig = formLayoutConfig;
    this.formEditReadOnlyMode = formEditReadOnlyMode;
    this.storeKey = storeKey;
    this.useCalcMode = useCalcMode;
    this.useSignalR = useSignalR;
    this.useView = useView;
    this.tableStateKey = tableStateKey;
    this.useViewTeamWithTypeId = useViewTeamWithTypeId;
    this.usePopup = usePopup;
    this.useSplit = useSplit;
    this.useOfflineMode = useOfflineMode;
    this.optionFilter = optionFilter;
    this.importMode = importMode;
    this.useImport =
      importMode?.useDelete === true ||
      importMode?.useInsert === true ||
      importMode?.useUpdate === true;
    this.useCompactMode = useCompactMode;
    this.useVirtualScroll = !!useVirtualScroll;
    this.useResizableColumn = useResizableColumn;
    if (showIcons) {
      this.showIcons = { ...this.showIcons, ...showIcons };
    }
    this.isFixable = isFixable ?? false;
    this.isCloneable = isCloneable ?? false;
    this.hasReadView = hasReadView ?? false;
  }
}
