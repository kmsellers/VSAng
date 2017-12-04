import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpClientModule} from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';


//import { NavBarComponent } from './components/navbar/navbar.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { Tabs} from './components/tabs/tabs.component';
import { Tab } from './components/tabs/tab.component';
import { TabItem } from './components/tabs/tab-item';
import { DynamicTabsDirective } from './components/tabs/dynamic-tabs.directive';
import { TabHostDirective } from './components/tabs/tab-host.directive';

@NgModule({

    imports: [
        CommonModule,
        ReactiveFormsModule,
        HttpClientModule,
    ],
    declarations: [
        HeaderComponent,
        //NavBarComponent,
        FooterComponent,
        Tabs,
        Tab,
        DynamicTabsDirective,
        TabHostDirective,
        
    ],
    exports: [CommonModule,
        ReactiveFormsModule,
        HttpClientModule,
        HeaderComponent,
        //NavBarComponent,
        FooterComponent,
        Tabs,
        Tab,
        DynamicTabsDirective,
        TabHostDirective
    ],
    entryComponents: [Tab],


})
export class AppModuleShared {
}
