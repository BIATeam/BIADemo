import { TeamTypeId } from "src/app/shared/constants";
import { PrimeTableColumn } from "../../../components/table/bia-table/bia-table-config";


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
  columns: PrimeTableColumn[];

  constructor({featureName,
    columns,
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
      columns: PrimeTableColumn[],
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
    this.columns = columns;
  }
}