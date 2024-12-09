import { NgModule } from '@angular/core';
import { BiaSharedModule } from './bia-shared/bia-shared.module';
import { MyDb } from './db/my-db/my-db.db';
import { DocumentRepository } from './db/my-db/repositories/document.repository';
import { UserRepository } from './db/my-db/repositories/user.repository';

const MODULES = [BiaSharedModule];

@NgModule({
  imports: MODULES,
  exports: [...MODULES],
  providers: [MyDb, UserRepository, DocumentRepository],
})

// https://medium.com/@benmohamehdi/angular-best-practices-coremodule-vs-sharedmodule-25f6721aa2ef
export class SharedModule {}
