
import {
  ComponentFactoryResolver,
  ComponentRef,
  Injectable,
  Type,
  ViewContainerRef
} from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class BiaInjectorService {
  constructor(private factoryResolver: ComponentFactoryResolver) {
    this.factoryResolver = factoryResolver;
  }
  addDynamicComponent<T>(viewContainerRef: ViewContainerRef, componentType: Type<T>): ComponentRef<T> {
    const factory = this.factoryResolver.resolveComponentFactory(componentType);
    return viewContainerRef.createComponent(factory);
  }
}
