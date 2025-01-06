import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { HomeIndexComponent } from './home-index.component';

export const HOME_ROUTES: Routes = [
  {
    path: '',
    component: HomeIndexComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  declarations: [HomeIndexComponent],
  imports: [SharedModule, TranslateModule.forChild(), RouterModule],
})
export class HomeModule {}
