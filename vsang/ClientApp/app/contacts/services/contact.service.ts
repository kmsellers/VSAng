import { Injectable, Inject, } from '@angular/core';
import { Http, Headers } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { Contact } from '../models/contact';

@Injectable()
export class ContactService
{

      private headers = new Headers({ 'Content-Type': 'application/json'});
      private contactsUrl = this.baseUrl + 'api/Contacts';  // URL to web api

      constructor(private http: Http, @Inject('BASE_URL') private baseUrl : string) { }

       getContacts(): Observable<Contact[]> {
          
           return this.http.get(this.contactsUrl).map(data => data.json() as Contact[]);

        }

      //async getContactOverview(id: string)
      //{
      //    const url = `${this.contactsUrl}/${id}/overview`;

      //    return await this.http.get(url).toPromise()
      //        .then(response => response.json() as Contact)
      //        .catch(this.handleError);
      //}

      //async getContactHistory(id: string)
      //{
      //    const url = `${this.contactsUrl}/${id}/history`;

      //    return await this.http.get(url).toPromise()
      //        .then(response => response.json() as Contact)
      //        .catch(this.handleError);
      //}

      getContact(id: string): Observable<Contact> {
          const url = `${this.contactsUrl}/${id}`;

          return this.http.get(url).map(data => data.json() as Contact)
             .catch(this.handleError);
      }

      delete(id: string): Promise<void> {
        const url = `${this.contactsUrl}/${id}`;
        return this.http.delete(url, {headers: this.headers})
          .toPromise()
          .then(() => null)
          .catch(this.handleError);
      }

      //create(email: string): Promise<Contact> {
      //    console.log("in service: " + email); 
      //    console.log("in service: " + JSON.stringify({ email: email })); 
      //  return this.http
      //      .post(this.contactsUrl, JSON.stringify({ email: email }), { headers: this.headers }) 
      //    .toPromise()
      //    .then(res => res.json().data as Contact)
      //    .catch(this.handleError);
      //}
      create(contact: Contact): Observable<Contact> {
          console.log("in service: " + contact.email);
          var myContact = JSON.stringify(contact);
          console.log("in service: " + myContact);
          return this.http
              .post(this.contactsUrl,myContact , { headers: this.headers })
              .catch(this.handleError);
      }


      update(contact: Contact): Observable<Contact> {
        const url = `${this.contactsUrl}/${contact.id}`;
        return this.http.put(url, JSON.stringify(contact), {headers: this.headers})
          .catch(this.handleError);
      }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
