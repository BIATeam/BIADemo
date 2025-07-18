import { OptionDto } from 'biang/models';

export class DictOptionDto {
  key: string;
  value: OptionDto[];

  constructor(key: string, value: OptionDto[]) {
    this.key = key;
    this.value = value;
  }
}
