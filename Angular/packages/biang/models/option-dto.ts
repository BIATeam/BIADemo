import { DtoState } from 'biang/models/enum';
import { BaseDto } from './dto/base-dto';

export class OptionDto extends BaseDto {
  display: string;

  constructor(id: any, display: string, dtoState: DtoState) {
    super(id, dtoState);
    this.display = display;
  }
}
