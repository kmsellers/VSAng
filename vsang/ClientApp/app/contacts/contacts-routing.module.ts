import { NgModule }             from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ContactsComponent } from './components/contact-list/contact-list.component';
import { ContactFormComponent } from "./components/contact-form/contact-form.component";
import { ContactComponent } from "./components/contact/contact.component";
import { ContactOverviewComponent } from "./components/contact-overview/contact-overview.component";
import { ContactAppComponent } from "./components/contact-app/contact-app.component";
import { ContactHistoryComponent } from "./components/contact-history/contact-history.component";

@NgModule({

    imports: [RouterModule.forChild([
        { path: '', component: ContactsComponent} ,
        //{ path: 'add', component: ContactFormComponent },
        { path: ':contactId', component: ContactAppComponent,
            //children: [
            //    { path: 'details', component: ContactComponent },
            //    { path: 'overview', component: ContactOverviewComponent },
            //    { path: 'history', component: ContactHistoryComponent },
            //]
        },
    ])],

    exports: [RouterModule]
})
export class ContactsRoutingModule {}