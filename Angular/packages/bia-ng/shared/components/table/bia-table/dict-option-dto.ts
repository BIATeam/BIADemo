import { OptionDto } from 'packages/bia-ng/models/public-api';

export class DictOptionDto {
  key: string;
  value: OptionDto[];

  constructor(key: string, value: OptionDto[]) {
    this.key = key;
    this.value = value;
  }
}
