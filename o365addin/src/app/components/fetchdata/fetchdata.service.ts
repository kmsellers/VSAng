import { Injectable, Inject } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from "rxjs/observable/of";
import { WeatherForecast } from './weatherforecast';
import 'rxjs/add/operator/map';


interface WFResponse 
{
    results: WeatherForecast[]
}

@Injectable()
export class FetchDataService {

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private forecastUrl = this.baseUrl + 'api/SampleData/WeatherForecasts';  // URL to web api

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }


    getWeatherForecast(): Observable<WeatherForecast[]> {
        return (this.http.get(this.forecastUrl)).map(data => data as WeatherForecast[]);
    }


    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }
}


