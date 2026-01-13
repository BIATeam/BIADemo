import { MenuItem } from 'primeng/api';

export interface BiaButtonAndMenuItem extends MenuItem {
  labelAsTooltip?: boolean;
  buttonClass?: string;
  menuOnlySeparator?: boolean;
  order?: number;
  withThrottleClick?: boolean;
  buttonOutlined?: boolean;
}
