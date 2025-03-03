import { BaseDto } from './base-dto';
import { DtoState } from './dto-state.enum';

export class FixableDto extends BaseDto {
  isFixed: boolean;
  fixedDate: Date;

  constructor(
    id: AnalyserNode,
    dtoState: DtoState,
    isFixed: boolean,
    fixedDate: Date
  ) {
    super(id, dtoState);
    this.isFixed = isFixed;
    this.fixedDate = fixedDate;
  }
}
