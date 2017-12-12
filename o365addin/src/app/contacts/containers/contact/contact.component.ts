import { Component, Inject, OnInit, OnDestroy, ViewChild, Input, ChangeDetectionStrategy } from '@angular/core';
import { ITabComponent } from "../../../components/tabs/tab-component.interface";
import { Contact } from "../../models/contact";
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ContactService } from '../../services/contact.service';

@Component({
    selector: 'app-contact-details',
    templateUrl: './contact.component.html',
    styleUrls: ['./contact.component.css'],


})
export class ContactComponent implements OnInit, ITabComponent {
    contactId: string;
    @Input() data: any;
    contact: Contact;

    constructor(private route: ActivatedRoute,private contactService: ContactService ) {

     }

    ngOnInit(): void {
        this.route.parent.params.subscribe(params => {
            this.contactId = params.contactId; 
            this.contactService.getContact(this.contactId).subscribe(c => this.contact = c); 
        });
    }
}

