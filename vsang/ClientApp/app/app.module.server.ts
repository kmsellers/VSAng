import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';
import { ContactsModule } from './contacts/contacts.module';
/* Routing Module */
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from "./core/core.module";
import { Tab } from "./components/tabs/tab.component";

@NgModule({

    imports: [
        ServerModule,
        AppModuleShared,
        AppRoutingModule,
        ContactsModule,
        CoreModule.forRoot({ userName: 'Module Server' }),

    ],
    declarations: [
        AppComponent,
    ],
    entryComponents: [ Tab ],
    bootstrap: [AppComponent]

})
export class AppModule {
}
