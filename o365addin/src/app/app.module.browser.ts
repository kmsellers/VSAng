import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';


/* Routing Module */
import { AppRoutingModule } from './app-routing.module';
import { environment } from '../environments/environment';
import { CoreModule } from './core/core.module';


@NgModule({
    imports: [
        BrowserModule,
        AppModuleShared,
        CoreModule.forRoot({userName: 'Module Browser'}),
        AppRoutingModule
    ],
    declarations: [
        AppComponent,
    ],
    bootstrap: [AppComponent],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl },
        { provide: 'BASE_API_URL', useFactory: getApiUrl },
       
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}

export function getApiUrl() {
    var apiUrl : string = environment.production ? getBaseUrl() : 'http://localhost:3000/';
    console.log("apiUrl = " + apiUrl );
    return apiUrl; 
}
