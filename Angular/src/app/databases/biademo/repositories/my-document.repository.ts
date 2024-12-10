import { Injectable } from '@angular/core';
import { BiaRepository } from 'src/app/core/bia-core/data/bia.repository';
import { BiaDemoDatabase } from '../biademo.database';
import { MyDocument } from '../entities/document.entity';

@Injectable()
export class MyDocumentRepository extends BiaRepository<MyDocument, string> {
  constructor(db: BiaDemoDatabase) {
    super(db.documents);
  }
}
