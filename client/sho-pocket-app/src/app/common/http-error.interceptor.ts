import { Injectable } from '@angular/core';
import {
    HttpEvent,
    HttpInterceptor,
    HttpHandler,
    HttpRequest,
    HttpErrorResponse
   } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ResponseError } from '../models/response-error.model';

@Injectable({
    providedIn: 'root'
})
export class HttpErrorInterceptor implements HttpInterceptor {

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((errorResponse: HttpErrorResponse) => {
                if ((errorResponse.status === 401 || errorResponse.status === 403) && (window.location.href.match(/\?/g) || []).length < 2) {
                    
                    console.log('The authentication session expired or the user is not authorized. Force refresh of the current page.');
                    /* Great solution for bundling with Auth Guard! 
                    1. Auth Guard checks authorized user (e.g. by looking into LocalStorage). 
                    2. On 401/403 response you clean authorized user for the Guard (e.g. by removing coresponding parameters in LocalStorage). 
                    3. As at this early stage you can't access the Router for forwarding to the login page,
                    4. refreshing the same page will trigger the Guard checks, which will forward you to the login screen */
                    localStorage.removeItem('auth_token');              
                    window.location.href = window.location.href + '?' + new Date().getMilliseconds();
                }

                if (errorResponse.statusText && errorResponse.statusText === "Unknown Error") {
                    let error: ResponseError = {
                        code: "0",
                        description: "Uknown error. Please contact the system administrator."
                    }

                    return throwError([error]);
                }
                
                return throwError(errorResponse.error);
            })
        )
    }

}