import { TeamTypeId } from "src/app/shared/constants";
import { BiaFieldsConfig } from "../../../model/bia-field-config";


export class CrudConfig {
  featureName: string;
  storeKey: string;
  useCalcMode: boolean;
  useSignalR: boolean;
  useView: boolean;
  tableStateKey: string;
  useViewTeamWithTypeId: TeamTypeId;
  usePopup: boolean;
  useOfflineMode: boolean;
  fieldsConfig: BiaFieldsConfig;

  constructor({featureName,
    fieldsConfig,
    storeKey = 'feature-' + featureName,
    useCalcMode = false,
    useSignalR = false,
    useView = false,
    tableStateKey = featureName + 'Grid',
    useViewTeamWithTypeId = TeamTypeId.Site,
    usePopup = true,
    useOfflineMode = false,
    } :
    {
      featureName: string,
      fieldsConfig: BiaFieldsConfig,
      storeKey?: string,
      useCalcMode?: boolean,
      useSignalR?: boolean,
      useView?: boolean,
      tableStateKey?: string,
      useViewTeamWithTypeId?: TeamTypeId,
      usePopup?: boolean,
      useOfflineMode?: boolean,
    })
  {
    this.featureName = featureName;
    this.storeKey = storeKey;
    this.useCalcMode = useCalcMode;
    this.useSignalR = useSignalR;
    this.useView = useView;
    this.tableStateKey = tableStateKey;
    this.useViewTeamWithTypeId = useViewTeamWithTypeId;
    this.usePopup = usePopup;
    this.useOfflineMode = useOfflineMode;
    this.fieldsConfig = fieldsConfig;
  }
}