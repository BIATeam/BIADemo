import { Injectable } from '@angular/core';
import { BiaRepository } from 'src/app/core/bia-core/data/bia.repository';
import { BiaDemoDatabase } from '../biademo.database';
import { User } from '../entities/user.entity';

@Injectable()
export class UserRepository extends BiaRepository<User, number> {
  constructor(db: BiaDemoDatabase) {
    super(db.users);
  }
}
