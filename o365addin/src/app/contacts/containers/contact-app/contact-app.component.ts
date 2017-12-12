import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/fromPromise';
import "rxjs/add/operator/filter";
import "rxjs/add/operator/pairwise";
import { Subscription } from "rxjs/Subscription";

import { Contact } from '../../models/Contact';
import { ContactService } from '../../services/contact.service';
import { ContactAddinService } from './contact-addin.service';

import { Tabs } from '../../../components/tabs/tabs.component';
import { ITabComponent } from '../../../components/tabs/tab-component.interface';
import { TabItem } from '../../../components/tabs/tab-item';


@Component({
  selector: 'app-contact-app',
  templateUrl: './contact-app.component.html'
  ,
  styleUrls: ['./contact-app.component.css'],
  providers: [ContactAddinService]
})
export class ContactAppComponent implements OnInit {

    contact: Contact;
    contactSub: Subscription;
    contactId: string; 
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

        this.contactSub = this.getContact(this.contactId)
            .subscribe(c => this.setContact(c), error => {

                console.log(error);

            }, () => { console.log("contact app contact subscribe completed. ")});
    }

    ngOnDestroy() {
        if (this.contactSub) {
            this.contactSub.unsubscribe();
        }
    }

 


    getContact(id: string): Observable<Contact> {

        return this.contactService.getContact(id);
    }

  setContact(contact: Contact) {
      console.log("contact app setContact:");     
      this.contact = contact;

      console.log(contact);
      this.contactTabs.setTabItems(this.contactAddinService.getContactTabItems(this.contact));

  }
}
