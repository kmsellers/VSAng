import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';
/* Routing Module */
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from "./core/core.module";

@NgModule({

    imports: [
        ServerModule,
        AppModuleShared,
        AppRoutingModule,
        CoreModule.forRoot({ userName: 'Module Server' }),

    ],
    declarations: [
        AppComponent,
    ],
    bootstrap: [AppComponent]

})
export class AppModule {
}
