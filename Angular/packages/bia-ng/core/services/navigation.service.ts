import { Injectable } from '@angular/core';
import { AuthInfo, BiaNavigation } from 'packages/bia-ng/models/public-api';

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
        element.permissions.some(
          r => authInfo?.decryptedToken?.permissions?.indexOf(r) >= 0
        );
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
