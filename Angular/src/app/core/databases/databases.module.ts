import { NgModule } from '@angular/core';
import { BiaDemoDatabaseModule } from './biademo/biademo.database.module';

const DATABASES = [
  // Begin BIADemo
  BiaDemoDatabaseModule,
  // End BIADemo
];

@NgModule({
  imports: [DATABASES],
})
export class DatabasesModule {}
