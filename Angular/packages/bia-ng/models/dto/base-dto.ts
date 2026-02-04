import { DtoState } from '@bia-team/bia-ng/models/enum';

export class BaseDto<T extends number | string = number> {
  id: T; // the id could be a number, a string or a GUID
  dtoState: DtoState;

  constructor(id: T, dtoState: DtoState) {
    this.id = id;
    this.dtoState = dtoState;
  }
}
