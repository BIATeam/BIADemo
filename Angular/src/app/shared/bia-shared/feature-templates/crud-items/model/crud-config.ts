import { TeamTypeId } from 'src/app/shared/constants';
import { BiaFieldsConfig } from '../../../model/bia-field-config';
import { BiaTableState } from '../../../model/bia-table-state';

export class CrudConfig {
  featureName: string;
  storeKey: string;
  useCalcMode: boolean;
  useSignalR: boolean;
  useView: boolean;
  tableStateKey: string;
  useViewTeamWithTypeId: TeamTypeId | null;
  usePopup: boolean;
  useOfflineMode: boolean;
  fieldsConfig: BiaFieldsConfig;
  defaultViewPref: BiaTableState;
  optionFilter: any;
  useBulk: boolean;
  bulkMode?: {
    useInsert: boolean;
    useUpdate: boolean;
    useDelete: boolean;
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
    bulkMode,
  }: {
    featureName: string;
    fieldsConfig: BiaFieldsConfig;
    storeKey?: string;
    useCalcMode?: boolean;
    useSignalR?: boolean;
    useView?: boolean;
    tableStateKey?: string;
    useViewTeamWithTypeId?: TeamTypeId | null;
    usePopup?: boolean;
    useOfflineMode?: boolean;
    optionFilter?: any;
    bulkMode?: {
      useInsert: boolean;
      useUpdate: boolean;
      useDelete: boolean;
    };
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
    this.bulkMode = bulkMode;
    this.useBulk =
      bulkMode?.useDelete === true ||
      bulkMode?.useInsert === true ||
      bulkMode?.useUpdate === true;
  }
}
