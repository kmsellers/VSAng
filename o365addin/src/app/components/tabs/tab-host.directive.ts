import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[tab-host]'
})
export class TabHostDirective {
  constructor(public viewContainer: ViewContainerRef){}
}