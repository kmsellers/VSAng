import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, UrlSegmentGroup, UrlSegment, PRIMARY_OUTLET, UrlTree } from '@angular/router';
import { Contact } from '../../models/contact';
import { ContactService } from '../../services/contact.service';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import { Subscription } from "rxjs/Subscription";

@Component({
  selector: 'app-contacts',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css'],

})
export class ContactsComponent implements OnInit, OnDestroy {
    contactsSub: Subscription;
    contacts: Contact[];
    selectedContact: any;
    email:string 

    constructor( @Inject('BASE_URL') private baseUrl: string,
        private contactService: ContactService, // private userService: UserService,
        private router: Router, 
        private route: ActivatedRoute) {

            const tree: UrlTree = this.router.parseUrl(router.url);
            const g: UrlSegmentGroup = tree.root.children[PRIMARY_OUTLET];
            const s: UrlSegment[] = g.segments;
            console.log(s);
            if (s.length === 3 && s[1].path === 'search')
            {
                console.log (s[2]);
                this.email =s[2].path;
                console.log(this.email);
            }


        }

    getContacts(): Observable<Contact[]> {
        return this.contactService
            .getContacts();
    }
    getContactsByEmail(email: string): Observable<Contact[]> {
        return this.contactService
            .getContactsByEmail(email);
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
        if (this.email === undefined)
        {
            this.contactsSub = this.getContacts()
                .subscribe(c => this.contacts = c, error => {
                    console.log(error);

            });
        }
        else {
            console.log("trying to subscribe to get contacts by email");
            this.contactsSub = this.getContactsByEmail(this.email)
            .subscribe(c => 
                {
                    console.log("get contacts by email");
                    
                    this.contacts = c;
                    console.log(this.contacts.length);
                    if (this.contacts.length === 0)
                    {
                        //got to add 
                        this.router.navigate(['add']); //need to pass email default
                    }
                    if (this.contacts.length ===1)
                    {
                        this.onViewDetails(this.contacts[0]);
                    }
                }, error => {
                console.log(error);

        });
        }
        
        
    }

    onSelect(contact: Contact): void {
        this.selectedContact = contact;
    }



    ngOnDestroy(): void {
        if (this.contactsSub)
            this.contactsSub.unsubscribe();
    }
}
