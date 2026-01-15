import { DtoState } from 'packages/bia-ng/models/enum/public-api';
import { BaseDto } from './dto/base-dto';

export class OptionDto extends BaseDto {
  display: string;

  constructor(id: any, display: string, dtoState: DtoState) {
    super(id, dtoState);
    this.display = display;
  }
}
