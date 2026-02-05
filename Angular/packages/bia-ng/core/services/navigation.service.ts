import { Injectable } from '@angular/core';
import { AuthInfo, BiaNavigation } from 'packages/bia-ng/models/public-api';
import { OptionPermission } from 'src/app/shared/option-permission';
import { Permission } from 'src/app/shared/permission';
import { BiaPermission } from '../bia-permission';

@Injectable({
  providedIn: 'root',
})
export class NavigationService {
  public filterNavByRole(
    authInfo: AuthInfo,
    biaNavigation: BiaNavigation[]
  ): BiaNavigation[] {
    const biaNavigationFiltered = new Array<BiaNavigation>();
    biaNavigation.forEach((element: BiaNavigation) => {
      const found =
        !element.permissions ||
        element.permissions.some(r => {
          // Convert enum to its string name if needed
          const permName =
            typeof r === 'number'
              ? Permission[r] || OptionPermission[r] || BiaPermission[r]
              : r;
          return authInfo?.decryptedToken?.permissions?.indexOf(permName) >= 0;
        });
      if (found) {
        biaNavigationFiltered.push(element);
        if (element.children) {
          element.children = this.filterNavByRole(authInfo, element.children);
        }
      }
    });

    return biaNavigationFiltered;
  }
}
