import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Contact } from '../../models/contact';
import { ContactService } from '../../services/contact.service';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import { Subscription } from "rxjs/Subscription";
//import { UserService } from "../../../core/user.service";

@Component({
  selector: 'app-contacts',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css'],

})
export class ContactsComponent implements OnInit, OnDestroy {
    contactsSub: Subscription;
    contacts: Contact[];
    selectedContact: any;

    constructor( @Inject('BASE_URL') private baseUrl: string,
        private contactService: ContactService, // private userService: UserService,
        private router: Router) { }

    getContacts(): Observable<Contact[]> {
        return this.contactService
            .getContacts();
    }

    newContact(): void {
        this.router.navigate(['/contacts/add']);
    }
    onContactDelete(contact : Contact) {
        console.log("in delete");
    //    this.delete(contact);
    }
    onViewDetails(contact: Contact): void {
        this.selectedContact = contact; 
        console.log("is selectedContact defined? " + this.selectedContact !== undefined && this.selectedContact !== null);
        console.log("is selectedContact id " + this.selectedContact.id );
        this.router.navigate(['/contacts', this.selectedContact.id]);
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



    ngOnDestroy(): void {
        if (this.contactsSub)
            this.contactsSub.unsubscribe();
    }
}
