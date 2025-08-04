import { DtoState } from 'bia-ng/models/enum';

export class BaseDto {
  id: any; // the id could be a number, a string or a GUID
  dtoState: DtoState;

  constructor(id: any, dtoState: DtoState) {
    this.id = id;
    this.dtoState = dtoState;
  }
}
