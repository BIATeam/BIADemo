import { DtoState } from './dto-state.enum';

export class BaseDto {
  id: any; // the id could be a number, a string or a GUID
  dtoState: DtoState;
}
