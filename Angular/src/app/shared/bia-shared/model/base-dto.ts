import { DtoState } from './dto-state.enum';

export class BaseDto {
  id: any; // the id could be a number, a string or a GUID
  dtoState: DtoState;
  rowVersion: string | undefined;
  isFixed: boolean | undefined;
  fixedDate: Date | undefined;

  constructor(id: AnalyserNode, dtoState: DtoState) {
    this.id = id;
    this.dtoState = dtoState;
  }
}
