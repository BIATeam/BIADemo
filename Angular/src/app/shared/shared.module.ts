import { NgModule } from '@angular/core';
import { BiaSharedModule } from './bia-shared/bia-shared.module';

const MODULES = [BiaSharedModule];

@NgModule({
  imports: MODULES,
  exports: [...MODULES]
})

// https://medium.com/@benmohamehdi/angular-best-practices-coremodule-vs-sharedmodule-25f6721aa2ef
export class SharedModule {}
