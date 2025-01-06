import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { NGXLogger } from 'ngx-logger';

@Injectable()
export class BiaErrorHandler implements ErrorHandler {
  protected logger: NGXLogger;

  constructor(protected injector: Injector) {
    this.logger = this.injector.get<NGXLogger>(NGXLogger);
  }

  handleError(error: Error | HttpErrorResponse) {
    this.logger.error(error);
  }
}
