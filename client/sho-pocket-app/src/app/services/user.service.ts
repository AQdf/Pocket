import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';

import { UserDetailsService } from './user-details.service'
import { UserRegistration } from'../models/user-registration.model'

const usersApiUrl = environment.baseApiUrl + 'users/';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private loggedIn = false;

  constructor(private http: Http, private userDetailsService: UserDetailsService) {
    this.loggedIn = !!localStorage.getItem('auth_token');
    // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
    // header component resulting in authed user nav links disappearing despite the fact user is still logged in
    this._authNavStatusSource.next(this.loggedIn);
  }

  register(email: string, password: string, firstName: string, lastName: string, location: string): Observable<UserRegistration> {
    let body = JSON.stringify({ email, password, firstName, lastName,location });
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let requestOptions = new RequestOptions({ headers: headers });

    return this.http.post(usersApiUrl + 'register', body, requestOptions).pipe(
      map(response => response.json())
    );
  }  

  login(email, password) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let body = JSON.stringify({ email, password });

    return this.http.post(usersApiUrl + 'login', body , { headers }).pipe(
      map(response => response.json()),
      map(response => {
        localStorage.setItem('auth_token', response.auth_token);
        this.loggedIn = true;
        this._authNavStatusSource.next(true);

        this.userDetailsService.getUserDetails().subscribe(userDetails => {
          localStorage.setItem('default_currencies', JSON.stringify(userDetails.defaultCurrencies));
          localStorage.setItem('primary_currency', userDetails.primaryCurrency);
        });

        return true;
      })
    );
  }

  logout() {
    localStorage.removeItem('auth_token');
    this.loggedIn = false;
    this._authNavStatusSource.next(false);
  }

  isLoggedIn() {
    return this.loggedIn;
  }
}
