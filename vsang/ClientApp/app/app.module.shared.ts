import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';  // <-- #1 import module
import { Http } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { ContactsComponent } from './contacts/components/contact-list/contact-list.component';
import { ContactFormComponent } from './contacts/components/contact-form/contact-form.component';
import { ContactCardComponent } from './contacts/components/contact-card/contact-card.component';
import { NavBarComponent } from './components/navbar/navbar.component';
import { ContactComponent } from './contacts/components/contact/contact.component';
import { ContactOverviewComponent } from './contacts/components/contact-overview/contact-overview.component';
import { ContactHistoryComponent } from './contacts/components/contact-history/contact-history.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { Tabs} from './components/tabs/tabs.component';
import { Tab } from './components/tabs/tab.component';
import { TabItem } from './components/tabs/tab-item';
import { DynamicTabsDirective } from './components/tabs/dynamic-tabs.directive';
import { TabHostDirective } from './components/tabs/tab-host.directive';

@NgModule({
    declarations: [
        AppComponent,
        ContactsComponent,
        ContactCardComponent,
        ContactFormComponent,
        ContactComponent,
        HeaderComponent,
        NavBarComponent,
        ContactOverviewComponent,
        ContactHistoryComponent,
        FooterComponent,
        Tabs,
        Tab,
        DynamicTabsDirective,
        TabHostDirective

    ],
    entryComponents: [Tab, ContactOverviewComponent, ContactHistoryComponent],
    imports: [
        CommonModule,
        Http,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'contact-list', pathMatch: 'full' },
            //{ path: 'contact-list', component: ContactsComponent, pathMatch: 'full' }, 
          //  { path: 'contacts/add', component: ContactFormComponent }, 
          //   { path: 'contact/:contactId', component: ContactComponent,
          //          children: [
          //              { path: '', component: ContactOverviewComponent },
          //              { path: 'contact-overview', component: ContactOverviewComponent },
          //          ]
          //  },
          //{ path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
