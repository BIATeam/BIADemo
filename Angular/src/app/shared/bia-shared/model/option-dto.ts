import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { DtoState } from './dto-state.enum';

export class OptionDto extends BaseDto {
  display: string;

  constructor(id: any, display: string, dtoState: DtoState) {
    super(id, dtoState);
    this.display = display;
  }
}
