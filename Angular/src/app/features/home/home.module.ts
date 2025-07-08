import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { HomeIndexComponent } from './home-index.component';

export const HOME_ROUTES: Routes = [
  {
    path: '',
    component: HomeIndexComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [TranslateModule.forChild(), RouterModule],
})
export class HomeModule {}
