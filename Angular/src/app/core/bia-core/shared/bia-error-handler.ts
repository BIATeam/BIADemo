import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { NGXLogger } from 'ngx-logger';

@Injectable()
export class BiaErrorHandler implements ErrorHandler {
  private logger: NGXLogger;

  constructor(private injector: Injector) {
    this.logger = this.injector.get<NGXLogger>(NGXLogger);
  }

  handleError(error: Error | HttpErrorResponse) {
    this.logger.error(error);
  }
}
