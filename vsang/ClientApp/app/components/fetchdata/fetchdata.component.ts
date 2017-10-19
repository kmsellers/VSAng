import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { FetchDataService } from './fetchdata.service';
import { WeatherForecast } from './weatherforecast';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/catch';
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html',
    providers: [FetchDataService]
})
export class FetchDataComponent implements OnInit {
    forecastSub: Subscription;
    forecasts: WeatherForecast[];
 
    // private baseUrl: string;
    constructor(private fetchdataService: FetchDataService) { }

    ngOnInit(): void {
        this.forecastSub = this.fetchdataService.getWeatherForecast()
            .subscribe(forecasts => this.forecasts = forecasts, error => {
                console.log(error);
            });
        //this.forecasts = this.fetchdataService.getWeatherForecast();
            //.catch(error => {
            //    // TODO: add real error handling
            //    console.log(error);
            //    return Observable.of<WeatherForecast[]>([]);
            //});
      //  this.forecasts.map(f => { console.log(f); });
    };

    ngOnDestroy(): void {
        this.forecastSub.unsubscribe(); 
    }
}

