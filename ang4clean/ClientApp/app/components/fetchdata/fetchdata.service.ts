import { Injectable, Inject } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { WeatherForecast } from './weatherforecast';
import 'rxjs/add/operator/map';

@Injectable()
export class FetchDataService {

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private forecastUrl = this.baseUrl + 'api/SampleData/WeatherForecasts';  // URL to web api

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) { }


    getWeatherForecast(): Observable<WeatherForecast[]> {
        return this.http.get(this.forecastUrl).map(response => response.json() as WeatherForecast[])
            .catch(error => this.handleError(error));

    }


    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }
}
