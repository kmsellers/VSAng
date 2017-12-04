import { Component, Inject, OnInit, OnDestroy, ViewChild, Input, ChangeDetectionStrategy } from '@angular/core';
import { ITabComponent } from "../../../components/tabs/tab-component.interface";
import { Contact } from "../../models/contact";


@Component({
    selector: 'app-contact-details',
    templateUrl: './contact.component.html',
    styleUrls: ['./contact.component.css'],


})
export class ContactComponent implements OnInit, ITabComponent {
    @Input() data: any;
    contact: Contact;

    constructor() { }

    ngOnInit(): void {
        this.contact = this.data; 
    }
}

