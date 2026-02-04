import { OptionDto } from '@bia-team/bia-ng/models';

export class DictOptionDto {
  key: string;
  value: OptionDto[];

  constructor(key: string, value: OptionDto[]) {
    this.key = key;
    this.value = value;
  }
}
