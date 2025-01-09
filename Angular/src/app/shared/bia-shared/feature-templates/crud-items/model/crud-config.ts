import { TeamTypeId } from 'src/app/shared/constants';
import { BiaFieldsConfig } from '../../../model/bia-field-config';
import { BiaTableState } from '../../../model/bia-table-state';

export interface ShowIconsConfig {
  showCalcMode: boolean;
  showPopup: boolean;
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
  useOfflineMode: boolean;
  fieldsConfig: BiaFieldsConfig<TDto>;
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
    showView: false,
    showSignalR: false,
    showCompactMode: false,
    showVirtualScroll: false,
    showResizableColumn: false,
  };

  constructor({
    featureName,
    fieldsConfig,
    storeKey = 'feature-' + featureName,
    useCalcMode = false,
    useSignalR = false,
    useView = false,
    tableStateKey = featureName + 'Grid',
    useViewTeamWithTypeId = null,
    usePopup = true,
    useOfflineMode = false,
    optionFilter = undefined,
    importMode,
    useCompactMode = false,
    useVirtualScroll = false,
    useResizableColumn = false,
    showIcons,
  }: {
    featureName: string;
    fieldsConfig: BiaFieldsConfig<TDto>;
    storeKey?: string;
    useCalcMode?: boolean;
    useSignalR?: boolean;
    useView?: boolean;
    tableStateKey?: string;
    useViewTeamWithTypeId?: TeamTypeId | null;
    usePopup?: boolean;
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
  }) {
    this.featureName = featureName;
    this.fieldsConfig = fieldsConfig;
    this.storeKey = storeKey;
    this.useCalcMode = useCalcMode;
    this.useSignalR = useSignalR;
    this.useView = useView;
    this.tableStateKey = tableStateKey;
    this.useViewTeamWithTypeId = useViewTeamWithTypeId;
    this.usePopup = usePopup;
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
  }
}
