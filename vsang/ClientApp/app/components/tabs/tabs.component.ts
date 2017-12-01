import {
    Component, ContentChildren, QueryList, OnInit, AfterContentInit, ViewChild,
    ComponentFactoryResolver, ViewContainerRef, Input, ComponentFactory, ChangeDetectionStrategy} from '@angular/core';


import { Tab } from './tab.component';
import { DynamicTabsDirective } from './dynamic-tabs.directive';
import { TabItem } from "./tab-item";
import { ITabComponent } from "./tab-component.interface";

@Component({
    selector: 'tabs',
    templateUrl: './tabs.component.html',
    styleUrls: ['./tabs.component.css']
})
export class Tabs implements OnInit, AfterContentInit {
    @Input() tabItems: TabItem[];

    dynamicTabs: Tab[] = [];

    @ContentChildren(Tab)
    tabs: QueryList<Tab>;

    @ViewChild(DynamicTabsDirective)
    dynamicTabPlaceholder: DynamicTabsDirective;

    /*
      Alternative approach of using an anchor directive
      would be to simply get hold of a template variable
      as follows
    */
    // @ViewChild('container', {read: ViewContainerRef}) dynamicTabPlaceholder;

    constructor(private _componentFactoryResolver: ComponentFactoryResolver) {

    }

    setTabItems(tabItems: TabItem[])
    {
        this.tabItems = tabItems;
        // get a component factory for our TabComponent
        if (tabItems) {
            var componentFactory = this._componentFactoryResolver.resolveComponentFactory(Tab);

            // fetch the view container reference from our anchor directive
            let viewContainerRef = this.dynamicTabPlaceholder.viewContainer;
            for (var ti of tabItems) {
                this.createTab(componentFactory, viewContainerRef, ti.title, ti.iconName, ti.ref, ti.component, ti.data, false);
            }

            this.defaultTabCheck();
        }
    }

    ngOnInit() {



    }

    // contentChildren are set
    ngAfterContentInit() {
        this.defaultTabCheck();
    }

    defaultTabCheck()
    {
        if (this.tabs && this.tabs.length) {
            // get all active tabs
            let activeTabs = this.tabs.filter((tab) => tab.active);

            // if there is no active tab set, activate the first
            if (activeTabs.length === 0) {
                this.selectTab(this.tabs.first);
                
            }
            return;
        }

        if (this.dynamicTabs && this.dynamicTabs.length)
        {
            let dynActiveTabs = this.dynamicTabs.filter((tab) => tab.active);

            // if there is no active tab set, activate the first
            if (dynActiveTabs.length === 0) {
                this.selectTab(this.dynamicTabs[0]);

            }
            return;
        }


    }

    openTab(title: string, iconName: string, ref: string, component : any, data : any, isCloseable = false) {
        // get a component factory for our TabComponent
        let componentFactory = this._componentFactoryResolver.resolveComponentFactory(Tab);

        // fetch the view container reference from our anchor directive
        let viewContainerRef = this.dynamicTabPlaceholder.viewContainer;

        this.createTab(componentFactory, viewContainerRef, title, iconName, ref, component, data, isCloseable);

        // set it active
        this.selectTab(this.dynamicTabs[this.dynamicTabs.length - 1]);
    }

    createTab(componentFactory: ComponentFactory<Tab>, viewContainerRef: any,
                title: string,  iconName: any, ref: any, component: any, data: any, isCloseable = false)
    {
        // alternatively...
        // let viewContainerRef = this.dynamicTabPlaceholder;

        // create a component instance
        let componentRef = viewContainerRef.createComponent(componentFactory);

        // set the according properties on our component instance
        let instance: Tab = componentRef.instance as Tab;
        instance.title = title;
        instance.iconName = iconName; 
        instance.ref = ref; 
        instance.component = component;
        instance.dataContext = data;
        instance.isCloseable = isCloseable;

        // remember the dynamic component for rendering the
        // tab navigation headers
        this.dynamicTabs.push(componentRef.instance as Tab);
     
    }

    selectTabByRef(ref: string)
    {
        var refTab = this.tabs.find(t => t.ref === ref);
        if (refTab !== undefined)
        {
            this.selectTab(refTab);
        }
        else 
        {
            throw "Error finding reference tab";
        }
    }

    selectTab(tab: Tab) {
        // deactivate all tabs
        this.tabs.toArray().forEach(tab => tab.deactivate());
        this.dynamicTabs.forEach(tab => tab.deactivate());

         

        // activate the tab the user has clicked on.
        tab.activate(); 

        //determine if dynamic Tab
        
    }

    closeTab(tab: Tab) {
        for (let i = 0; i < this.dynamicTabs.length; i++) {
            if (this.dynamicTabs[i] === tab) {
                // remove the tab from our array
                this.dynamicTabs.splice(i, 1);

                // destroy our dynamically created component again
                let viewContainerRef = this.dynamicTabPlaceholder.viewContainer;
                // let viewContainerRef = this.dynamicTabPlaceholder;
                viewContainerRef.remove(i);

                // set tab index to 1st one
                this.selectTab(this.tabs.first);
                break;
            }
        }
    }

    closeActiveTab() {
        let activeTabs = this.dynamicTabs.filter((tab) => tab.active);
        if (activeTabs.length > 0) {
            // close the 1st active tab (should only be one at a time)
            this.closeTab(activeTabs[0]);
        }
    }

}