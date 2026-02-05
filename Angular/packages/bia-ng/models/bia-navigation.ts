import { BiaPermission } from 'packages/bia-ng/core/bia-permission';
import { OptionPermission } from 'src/app/shared/option-permission';
import { Permission } from 'src/app/shared/permission';

export interface BiaNavigation {
  path?: string[];
  labelKey: string;
  children?: BiaNavigation[];
  permissions?: (string | Permission | OptionPermission | BiaPermission)[];
  icon?: string;
}
