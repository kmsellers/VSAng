import { NgModule }             from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ContactsComponent } from './containers/contact-list/contact-list.component';
import { ContactFormComponent } from "./containers/contact-form/contact-form.component";
import { ContactComponent } from "./containers/contact/contact.component";
import { ContactOverviewComponent } from "./containers/contact-overview/contact-overview.component";
import { ContactAppComponent } from "./containers/contact-app/contact-app.component";
import { ContactHistoryComponent } from "./containers/contact-history/contact-history.component";

@NgModule({

    imports: [RouterModule.forChild([
        { path: '', component: ContactsComponent, pathMatch: 'full'} ,
        { path: 'search/:email', component: ContactsComponent} ,
        { path: 'add', component: ContactFormComponent },
        { path: ':contactId', component: ContactAppComponent,
            children: [
                { path: '', redirectTo: 'details', pathMatch: 'full'},
                { path: 'details', component: ContactComponent },
                { path: 'overview/:id', component: ContactOverviewComponent },
                { path: 'overview/:id', component: ContactOverviewComponent },   
                { path: 'history', component: ContactHistoryComponent },
            ]
        },
    ])],

    exports: [RouterModule]
})
export class ContactsRoutingModule {}