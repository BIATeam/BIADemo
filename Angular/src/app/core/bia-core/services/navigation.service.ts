import { Injectable } from '@angular/core';
import { AuthInfo } from 'src/app/shared/bia-shared/model/auth-info';
import { BiaNavigation } from 'src/app/shared/bia-shared/model/bia-navigation';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {
  constructor() {}

  public filterNavByRole(authInfo: AuthInfo, biaNavigation: BiaNavigation[]): BiaNavigation[] {
    const biaNavigationFiltered = new Array<BiaNavigation>();
    biaNavigation.forEach((element: BiaNavigation, index: number) => {
      const found =
        !element.permissions ||
        element.permissions.some((r) => authInfo && authInfo.permissions && authInfo.permissions.indexOf(r) >= 0);
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
