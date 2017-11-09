import { Component, OnInit, Input, ViewChild, ComponentFactoryResolver } from '@angular/core';
import { ITabComponent } from "./tab-component.interface";
import { TabHostDirective } from "./tab-host.directive";

@Component({
  selector: 'tab',
  templateUrl: './tab.component.html',
  styleUrls: ['./tab.component.css']       
})
export class Tab implements OnInit {
    @Input() title: string;
    @Input() active : boolean = false;
    @Input() isCloseable = false;
    @Input() template : any;
    @Input() dataContext : any;
    @Input() iconName : any; 
    @Input() ref : any
    component: any; 

    @ViewChild(TabHostDirective) tabHost: TabHostDirective;

    constructor(private _componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit() {

    console.log("title " + this.title); 

  }

  activate()
  {
      let viewContainerRef = this.tabHost.viewContainer;
        //   viewContainerRef.clear();
      if (this.component !== undefined && this.template === undefined) {
          let componentFactory = this._componentFactoryResolver.resolveComponentFactory(this.component);


          let componentRef = viewContainerRef.createComponent(componentFactory);
          this.template = componentRef;
          (<ITabComponent>componentRef.instance).data = this.dataContext;
      }
      this.active = true; 

  }
  deactivate() {
      this.active = false; 
  }
}
