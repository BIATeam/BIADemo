import { RoleMode } from "../../constants";

export class  TeamLoginDto {
    teamTypeId: number;
    teamId: number;
    roleMode: RoleMode;
    roleIds: number[];
    useDefaultRoles: boolean;
}
  