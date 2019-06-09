import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { map } from 'rxjs/operators';

import { UserDetails } from'../models/user-details.model'
import { environment } from '../../environments/environment'

const userDetailsApiUrl = environment.baseApiUrl + 'user/details/';

@Injectable({
  providedIn: 'root'
})
export class UserDetailsService {

  constructor(private http: Http) { }

  getHeaders() {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return headers;
  }

  getUserDetails() {
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headers });

    return this.http.get(userDetailsApiUrl, requestOptions).pipe(
      map(data => {
        return data.json() as UserDetails;
      })
    );
  }
}
