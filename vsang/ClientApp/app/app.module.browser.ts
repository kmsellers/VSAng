import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './components/app/app.component';
import { ContactsModule } from './contacts/contacts.module';
import { CoreModule } from './core/core.module';

/* Routing Module */
import { AppRoutingModule } from './app-routing.module';
import { Tab } from "./components/tabs/tab.component";

@NgModule({
    imports: [
        BrowserModule,
        ContactsModule,
        CoreModule.forRoot({userName: 'Module Browser'}),
        AppRoutingModule
    ],
    declarations: [
        AppComponent,
    ],
    bootstrap: [AppComponent],
    entryComponents: [Tab],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl }
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
