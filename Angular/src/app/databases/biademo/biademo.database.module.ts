import { NgModule } from '@angular/core';
import { BiaDemoDatabase } from './biademo.database';
import { MyDocumentRepository } from './repositories/my-document.repository';
import { UserRepository } from './repositories/user.repository';

const DATABASE = BiaDemoDatabase;
const REPOSITORIES = [MyDocumentRepository, UserRepository];

@NgModule({
  providers: [DATABASE, REPOSITORIES],
})
export class BiaDemoDatabaseModule {}
