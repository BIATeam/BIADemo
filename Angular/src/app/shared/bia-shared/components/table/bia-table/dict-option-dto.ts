import { OptionDto } from '../../../model/option-dto';

export class DictOptionDto {
  key: string;
  value: OptionDto[];

  constructor(key: string, value: OptionDto[]) {
    this.key = key;
    this.value = value;
  }
}
