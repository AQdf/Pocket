import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';

import { environment } from '../../environments/environment';
import { UserDetailsService } from './user-details.service'
import { UserRegistration } from '../models/user-registration.model';
import { UserDetails } from '../models/user-details.model';

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

  constructor(private http: HttpClient, private userDetailsService: UserDetailsService) {
    this.loggedIn = !!localStorage.getItem('auth_token');
    // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
    // header component resulting in authed user nav links disappearing despite the fact user is still logged in
    this._authNavStatusSource.next(this.loggedIn);
  }

  register(userData: UserRegistration) {
    let body = JSON.stringify(userData);
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    let requestOptions = { headers: headers };

    return this.http.post(usersApiUrl + 'register', body, requestOptions);
  }  

  login(email, password) {
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    let body = JSON.stringify({ email, password });

    return this.http.post(usersApiUrl + 'login', body, {headers}).pipe(
      map((response:any) => {
        localStorage.setItem('auth_token', response.auth_token);
        this.loggedIn = true;
        this._authNavStatusSource.next(true);

        this.userDetailsService.getUserDetails().subscribe((userDetails: UserDetails) => {
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
