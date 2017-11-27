import { NgModule } from '@angular/core';

import { AppModuleShared } from '../../app/app.module.shared';
import { ContactsRoutingModule } from './contacts-routing.module';

import { ContactAppComponent } from './components/contact-app/contact-app.component';
import { ContactsComponent } from './components/contact-list/contact-list.component';
import { ContactFormComponent } from './components/contact-form/contact-form.component';
import { ContactCardComponent } from './components/contact-card/contact-card.component';
import { ContactComponent } from './components/contact/contact.component';
import { ContactOverviewComponent } from './components/contact-overview/contact-overview.component';
import { ContactHistoryComponent } from './components/contact-history/contact-history.component';

import { ContactService } from "./services/contact.service";

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
    entryComponents: [ContactComponent, ContactOverviewComponent, ContactHistoryComponent],

})
export class ContactsModule {
}
