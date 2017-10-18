import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Contact } from './contact';
import { ContactService } from './contact.service';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/fromPromise';
import { Subscription } from "rxjs/Subscription";

@Component({
  selector: 'app-contacts',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css'],
  providers: [ContactService]
})
export class ContactsComponent implements OnInit, OnDestroy {
    contactsSub: Subscription;
    contacts: Contact[];
    collcontacts: any; 
    selectedContact: any;

    constructor( @Inject('BASE_URL') private baseUrl: string,
        private contactService: ContactService,
        private router: Router) { }

    getContacts(): Observable<Contact[]> {
        return Observable.fromPromise(this.contactService
            .getContacts());
    }

    newContact(): void {
        this.router.navigate(['/contacts/add']);
    }

    delete(contact: Contact): void {
        this.contactService
            .delete(contact.id)
            .then(() => {
                this.contacts = this.contacts.filter(h => h !== contact);
                if (this.selectedContact === contact) { this.selectedContact = null; }
            });
    }

    ngOnInit(): void {
        this.contactsSub = this.getContacts()
            .subscribe(c => this.contacts = c, error => {
                console.log(error);

        });
    }

    onSelect(contact: Contact): void {
        this.selectedContact = contact;
    }

    gotoDetail(): void {
        this.router.navigate(['/detail', this.selectedContact.id]);
    }

    ngOnDestroy(): void {
        this.contactsSub.unsubscribe();
    }
}
