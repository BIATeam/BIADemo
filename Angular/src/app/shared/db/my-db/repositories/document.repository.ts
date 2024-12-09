import { Injectable } from '@angular/core';
import { BiaRepository } from 'src/app/core/bia-core/bia.repository';
import { MyDocument } from '../entities/document.entity';
import { MyDb } from '../my-db.db';

@Injectable()
export class DocumentRepository extends BiaRepository<MyDocument, string> {
  constructor(db: MyDb) {
    super(db.documents);
  }
}
