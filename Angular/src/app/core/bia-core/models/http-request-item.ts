import { HttpRequest } from '@angular/common/http';

export interface HttpRequestItem {
  id?: number;
  httpRequest: HttpRequest<any>;
}
