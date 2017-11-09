import { Injectable } from '@angular/core';

import { ContactOverviewComponent } from '../contact-overview/contact-overview.component';
import { ContactHistoryComponent } from '../contact-history/contact-history.component';
import { TabItem } from '../../../components/tabs/tab-item';

@Injectable()
export class ContactAddinService {
    getContactTabItems(id : string) {
        return [
            new TabItem('Overview', 'user', './overview', ContactOverviewComponent, { id : id }),
            new TabItem('History', 'th-list', './history', ContactHistoryComponent, { id : id }),

        ];
    }
}