import { DtoState } from 'packages/bia-ng/models/enum/public-api';

export class BaseDto {
  id: any; // the id could be a number, a string or a GUID
  dtoState: DtoState;

  constructor(id: any, dtoState: DtoState) {
    this.id = id;
    this.dtoState = dtoState;
  }
}
