import { Injectable } from '@angular/core';
import { BiaRepository } from 'src/app/core/bia-core/data/bia.repository';
import { User } from '../entities/user.entity';
import { MyDb } from '../my-db.db';

@Injectable()
export class UserRepository extends BiaRepository<User, number> {
  constructor(db: MyDb) {
    super(db.users);
  }
}
