import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export interface Member extends BaseDto {
  user: OptionDto;
  roles: OptionDto[];
  teamId: number;
}

export class Members {
  users: OptionDto[];
  roles: OptionDto[];
  teamId: number;
}
