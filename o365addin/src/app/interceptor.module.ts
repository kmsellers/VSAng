import { Injectable, NgModule } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HTTP_INTERCEPTORS, HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders } from '@angular/common/http';
import  'rxjs/add/operator/do';


@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {
        intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
            console.log("In Interceptor");
           if (!req.headers.has("Origin") || req.headers.get('Origin') === null)
           {
               req.headers.set('Origin','http://localhost:4200'); 
           }
           
            if (req.url.startsWith('http://localhost:3000'))
            {
                req.headers.set('Access-Control-Allow-Origin', 'http://localhost:3000')
            }
            if (req.url.startsWith('http://maxcdn.bootstrapcdn.com'))
            {
            const dupReq = req.clone( { headers: req.headers.set('Access-Control-Allow-Origin', 'http://maxcdn.bootstrapcdn.com')});
            return next.handle(dupReq);
            }
            const dupReq = req.clone( { headers: req.headers});
            return next.handle(dupReq); 
            
        }
}
@NgModule({
    providers:[{
        provide: HTTP_INTERCEPTORS,
        useClass: HttpRequestInterceptor,
        multi: true
    }],
})
export class InterceptorModule{}
