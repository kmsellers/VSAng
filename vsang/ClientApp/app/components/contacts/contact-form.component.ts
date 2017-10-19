import { Component, Inject, Input, OnInit } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

import { ContactService } from './contact.service';
import { Contact } from './contact'; 
import { States } from '../common/data-model';

@Component({
    selector: 'app-contacts-form',
    templateUrl: './contact-form.component.html',
    styleUrls: ['./contact-form.component.css'],
    providers: [ContactService]
})
export class ContactFormComponent implements OnInit {
    @Input() contact: Contact;
    contactForm: FormGroup;
    states = States;

    constructor(
        @Inject('BASE_URL') private baseUrl: string,
        private fb: FormBuilder,
        private contactService: ContactService,
        private router: Router) { }

    ngOnInit() {
        this.contactForm = this.fb.group(

            {
                first_name: '',
                last_name: '',
                email: '',
                mobile_phone: '',
                work_phone: '',
                address_line_1: '',
                city: '',
                state: '',
                postal_code: '',
                country: 'USA',
            });
    }

    prepareSaveContact(): any {
        const formModel = this.contactForm.value;
        console.log("pre save form model: " + JSON.stringify(formModel));
        // return new `Hero` object containing a combination of original hero value(s)
        // and deep copies of changed form model values
        const saveContact: any = {
            first_name: formModel.first_name as string,
            last_name: formModel.last_name as string,
            email: formModel.email as string,
            mobile_phone: formModel.mobile_phone as string,
            work_phone: formModel.work_phone as string,
            address_line_1: formModel.address_line_1 as string,
            city: formModel.city as string,
            state: formModel.state as string,
            postal_code: formModel.postal_code as string,
            country: "USA",
        };
        console.log("pre save form model: " + JSON.stringify(saveContact));

        return saveContact;
    }

    onSubmit() {
        console.log("submitting form");
        this.contact = this.prepareSaveContact();
        this.contactService.create(this.contact)
            .subscribe(/* error handling */);
        this.router.navigate(['/contacts/list']);
    }

}