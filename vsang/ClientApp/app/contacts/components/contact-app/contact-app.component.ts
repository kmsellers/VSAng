import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/fromPromise';
import { Subscription } from "rxjs/Subscription";

import { Contact } from '../../models/Contact';
import { ContactService } from '../../services/contact.service';
import { ContactAddinService } from './contact-addin.service';

import { Tabs } from '../../../components/tabs/tabs.component';
import { ITabComponent } from '../../../components/tabs/tab-component.interface';
import { TabItem } from '../../../components/tabs/tab-item';
//import { NavBarItem } from '../../../components/navbar/navbar-item';

@Component({
  selector: 'app-contact-app',
  templateUrl: './contact-app.component.html',
  styleUrls: ['./contact-app.component.css'],
  providers: [ContactAddinService]
})
export class ContactAppComponent implements OnInit {
    //Not sure why we have namespace -
    //- possibly so that the navbar can be added to from other namespaces? doesn't make sense
    namespace: string = 'sgacrExisting';
    //TODO:  Navbar - need to get navbar items from metadata and/or state. 
    //navbarItems: NavBarItem[]; 
    contact: Contact;
    contactSub: Subscription;     contactId: string; 
    tabItems: TabItem[];
     @ViewChild(Tabs) contactTabs: Tabs;

      constructor(private route: ActivatedRoute,
          private contactService: ContactService, private contactAddinService: ContactAddinService) {
          this.route.params.subscribe(params => {
              console.log(params);
              this.contactId = params.contactId;
              console.log("this contactId = " + this.contactId);
          });
}

      ngOnInit() {
           //this.getNavBarItems().subscribe(nbi => this.navbarItems = nbi, error => {
        //    console.log(error);
        //});

      this.contactSub = this.getContact(this.contactId)
          .subscribe(c => this.setContact(c), error => {
              console.log(error);

          });

      }
      ngOnDestroy() {
          this.contactSub.unsubscribe();
      }

      getContact(id: string): Observable<Contact> {

          return this.contactService.getContact(id);
      }
    //getNavBarItems(): Observable<NavBarItem[]> {
    //    return Observable.of([
    //        { title: 'Overview', iconName: 'user', uiSref: './overview' },
    //        { title: 'History', iconName: 'th-list', uiSref: './history' } //,
    //        //{ title: 'Comms', iconName: 'comment', uiSref: './comms' },
    //        //{ title: 'Notes', iconName: 'tasks', uiSref: './notes' },
    //        //{ title: 'Files', iconName: 'folder-open', uiSref: 'files' }
    //    ]);

    //}

  setContact(contact: Contact) {
      this.contact = contact;
      this.contactTabs.setTabItems(this.contactAddinService.getContactTabItems(this.contact));

  }
}
