import { BaseDto } from "src/app/shared/bia-shared/model/base-dto";
import { OptionDto } from "src/app/shared/bia-shared/model/option-dto";

export interface User extends BaseDto {
  lastName: string;
  firstName: string;
  login: string;
  guid: string;
  // Computed by Angular when data receive
  displayName: string;
  roles: OptionDto[];
}
