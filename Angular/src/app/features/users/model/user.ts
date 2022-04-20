import { OptionDto } from "src/app/shared/bia-shared/model/option-dto";

export interface User {
  id: number;
  lastName: string;
  firstName: string;
  login: string;
  guid: string;
  // Computed by Angular when data receive
  displayName: string;
  roles: OptionDto[];
}
