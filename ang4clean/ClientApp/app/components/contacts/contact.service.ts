import { Injectable, Inject }    from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { Contact } from './contact';

@Injectable()
export class ContactService
{

      private headers = new Headers({ 'Content-Type': 'application/json'});
      private contactsUrl = this.baseUrl + 'api/Contacts';  // URL to web api

      constructor(private http: Http, @Inject('BASE_URL') private baseUrl : string) { }

      async getContacts(): Promise<Contact[]> {

          let svcContacts: Contact[] = [{
             id:"some guid id",
              email: "k.m@sage.com",
              first_name: "k",
              last_name: "k",
              address_line_1: "addr 1",
              city: "city",
              state: "CA",
             postal_code: "92618",
              country: "USA"
          }]; 


          //const myPromise = () => {
          //    return new Promise((resolve, reject) => {
          //        resolve(svcContacts);
          //    })
          //}

          //return myPromise;  
          
          var response = await this.http.get(this.contactsUrl).toPromise();
          return response.json() as Contact[];

        }


      getContact(id: string): Promise<Contact> {
        const url = `${this.contactsUrl}/${id}`;
            return this.http.get(url)
              .toPromise()
              .then(response => response.json().data as Contact)
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
              .map(res => res.json() as Contact)
              .catch(this.handleError);
      }


      update(contact: Contact): Promise<Contact> {
        const url = `${this.contactsUrl}/${contact.id}`;
        return this.http
          .put(url, JSON.stringify(contact), {headers: this.headers})
          .toPromise()
          .then(() => contact)
          .catch(this.handleError);
      }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
