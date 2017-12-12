import { NgModule } from '@angular/core';

import { AppModuleShared } from '../../app/app.module.shared';

//TODO:  This should probably be in an environment specfic module.  In production, we do not use in mem services. 
//       Look at how sic handled the inmem for testing.

import { ContactsRoutingModule } from './contacts-routing.module';

import { ContactAppComponent } from './containers/contact-app/contact-app.component';
import { ContactsComponent } from './containers/contact-list/contact-list.component';
import { ContactFormComponent } from './containers/contact-form/contact-form.component';
import { ContactCardComponent } from './containers/contact-card/contact-card.component';
import { ContactComponent } from './containers/contact/contact.component';
import { ContactOverviewComponent } from './containers/contact-overview/contact-overview.component';
import { ContactHistoryComponent } from './containers/contact-history/contact-history.component';

import { ContactService } from "./services/contact.service";
import { Tab } from "../components/tabs/tab.component";


@NgModule({
    declarations: [
        ContactAppComponent,
        ContactsComponent,
        ContactCardComponent,
        ContactFormComponent,
        ContactComponent,
        ContactOverviewComponent,
        ContactHistoryComponent,
    ],
    imports: [
        AppModuleShared,
 
        ContactsRoutingModule
    ],
    providers : [ ContactService ],
    entryComponents: [Tab, ContactComponent, ContactOverviewComponent, ContactHistoryComponent],

})
export class ContactsModule {
}
