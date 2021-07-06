import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { HomeIndexComponent } from './home-index.component';
import { SharedModule } from 'src/app/shared/shared.module';

export const HOME_ROUTES: Routes = [
  {
    path: '',
    component: HomeIndexComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  declarations: [HomeIndexComponent],
  imports: [SharedModule, TranslateModule.forChild(), RouterModule]
})
export class HomeModule { }
