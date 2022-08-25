import { TeamTypeId } from "src/app/shared/constants";
import { PrimeTableColumn } from "../../../components/table/bia-table/bia-table-config";


export interface CrudConfig {
  storeKey: string,
  useCalcMode: boolean,
  useSignalR: boolean,
  useView: boolean,
  tableStateKey: string,
  useViewTeamWithTypeId: TeamTypeId,
  usePopup: boolean,
  columns: PrimeTableColumn[];
}
