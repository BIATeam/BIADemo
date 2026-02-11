import { Injectable, inject } from '@angular/core';
import { BiaNavigation } from 'packages/bia-ng/models/public-api';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class NavigationService {
  private authService = inject(AuthService);

  public filterNavByRole(biaNavigation: BiaNavigation[]): BiaNavigation[] {
    const biaNavigationFiltered = new Array<BiaNavigation>();
    biaNavigation.forEach((element: BiaNavigation) => {
      const found =
        !element.permissions ||
        element.permissions.some(permissionName => {
          return this.authService.hasPermission(permissionName);
        });
      if (found) {
        biaNavigationFiltered.push(element);
        if (element.children) {
          element.children = this.filterNavByRole(element.children);
        }
      }
    });

    return biaNavigationFiltered;
  }
}
