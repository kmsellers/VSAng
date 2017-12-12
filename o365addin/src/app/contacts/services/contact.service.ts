import { Injectable, Inject, } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { Contact } from '../models/contact';
import { SalesHistory } from '../models/sales-history';

@Injectable()
export class ContactService
{
    salesHistory: any;
    salesHistory$: any;
    contact: Contact;
    private contact$: Observable<Contact> = Observable.of(this.contact); 
    private id: string; 

      private headers = new HttpHeaders({ 'Content-Type': 'application/json'});
      private contactsUrl = this.baseUrl + 'api/contacts';  // URL to web api

      
      constructor(private http: HttpClient, @Inject('BASE_API_URL') private baseUrl : string) { }

       getContacts(): Observable<Contact[]> {
          
           return this.http.get(this.contactsUrl).map(data => data as Contact[]);

        }

        getContactsByEmail(email: string): Observable<Contact[]> {
            
            console.log("service call api contacts?email=email");
             var params : HttpParams = new HttpParams().set('email', email);

             
             return this.http.get(this.contactsUrl,{ params: params}).map(data => data as Contact[]);
  
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
            if (this.contactExists(id)) { 
                console.log("returning observable of contact");
                return Observable.of(this.contact);
            }
        
            
            this.contact = null; 
            this.salesHistory = null; 
            this.id = id; 
            const url = `${this.contactsUrl}/${id}`;
            console.log("calling get contact api");
            var contact$ = this.http.get(url).map(data => data as Contact)
              .catch(this.handleError);
            contact$.subscribe(c => this.contact = c, () => {console.log("contact completed");});
            return contact$; 
        }

        private contactExists(id: string)
        {
            return this.id === id && this.contact !== null;
        }

        getContactSalesHistory(id: string): Observable<SalesHistory> {
            
            if (!this.contactExists(id)) { 
                this.getContact(id).subscribe(c => {
                    return this.getContactSalesHistory(id);
                    });
            }
        
            if (this.salesHistory !== null)
            {
                return Observable.of(this.salesHistory); 
            }
            const url = `${this.contactsUrl}/${id}/sales-history`;

            this.salesHistory$ = this.http.get(url).map(data => data as SalesHistory)
              .catch(this.handleError);
            this.salesHistory$.subscribe(data => this.salesHistory = data, () => {console.log("sales history completed");});
            return this.salesHistory$; 
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

    private handleError(error: any): Observable<any> {
        console.error('An error occurred', error); // for demo purposes only
          return Observable.throw(error);;
      }
}
