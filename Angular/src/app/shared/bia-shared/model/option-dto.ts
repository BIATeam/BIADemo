import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { DtoState } from './dto-state.enum';

export class OptionDto extends BaseDto {
  display: string;

  constructor(id: any, display: string, dtoState: DtoState) {
    super(id, dtoState);
    this.display = display;
  }
}
