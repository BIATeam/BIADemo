import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { ExamplesComponent } from './examples.component';
import { ExamplesEffects } from './store/examples-effects';

const routes: Routes = [{ path: '', component: ExamplesComponent }];

@NgModule({
  imports: [
    CommonModule,
    ButtonModule,
    TooltipModule,
    RouterModule.forChild(routes),
    ExamplesComponent,
    EffectsModule.forFeature([ExamplesEffects]),
  ],
})
export class ExamplesModule {}
