import { Injectable } from '@angular/core';

import { ContactOverviewComponent } from '../contact-overview/contact-overview.component';
import { ContactHistoryComponent } from '../contact-history/contact-history.component';
import { TabItem } from '../../../components/tabs/tab-item';
import { ContactComponent } from "../contact/contact.component";
import { Contact } from "../../models/contact";

@Injectable()
export class ContactAddinService {
    getContactTabItems(contact : Contact) {
        return [
            new TabItem('Details', 'user', './details', ContactComponent, { contact: contact }),
            new TabItem('Overview', 'user', './overview', ContactOverviewComponent, { id : contact.id }),
            new TabItem('History', 'th-list', './history', ContactHistoryComponent, { id : contact.id }),

        ];
    }
}