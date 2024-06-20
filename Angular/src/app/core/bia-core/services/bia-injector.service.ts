import {
  ComponentRef,
  Injectable,
  Type,
  ViewContainerRef,
} from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BiaInjectorService {
  addDynamicComponent<T>(
    viewContainerRef: ViewContainerRef,
    componentType: Type<T>
  ): ComponentRef<T> {
    return viewContainerRef.createComponent(componentType);
  }
}
