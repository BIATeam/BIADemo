import { OptionDto } from 'bia-ng/models';

export class DictOptionDto {
  key: string;
  value: OptionDto[];

  constructor(key: string, value: OptionDto[]) {
    this.key = key;
    this.value = value;
  }
}
