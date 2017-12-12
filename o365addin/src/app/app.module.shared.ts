import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

import { Tabs} from './components/tabs/tabs.component';
import { Tab } from './components/tabs/tab.component';
import { TabItem } from './components/tabs/tab-item';
import { DynamicTabsDirective } from './components/tabs/dynamic-tabs.directive';
import { TabHostDirective } from './components/tabs/tab-host.directive';

//import { TableListComponent } from './components/table-list/table-list.component';
import { RouterModule } from '@angular/router';
import { InterceptorModule } from './interceptor.module';

@NgModule({

    imports: [
        RouterModule,
        CommonModule,
        ReactiveFormsModule,
        HttpClientModule,
        InterceptorModule,
    ],
    declarations: [
        Tabs,
        Tab,
        DynamicTabsDirective,
        TabHostDirective,
       // TableListComponent
        
    ],
    exports: [CommonModule,
        ReactiveFormsModule,
        HttpClientModule,
        InterceptorModule,
        Tabs,
        Tab,
        DynamicTabsDirective,
        TabHostDirective,
       // TableListComponent
    ],
    entryComponents: [Tab],


})
export class AppModuleShared {
}
