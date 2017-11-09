import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact';

@Component({
    selector: 'app-contact-card',
    templateUrl: './contact-card.component.html',
    styleUrls: ['./contact-card.component.css'],
    providers: [ContactService]
})
export class ContactCardComponent {
    @Input() contact : Contact;
    @Output() delete = new EventEmitter();
    @Output() view = new EventEmitter();
    constructor(private contactService: ContactService) { }

    onDelete() {
        this.delete.emit(this.contact);
    }
    onViewDetails() {
        this.view.emit(this.contact);
    }

}


