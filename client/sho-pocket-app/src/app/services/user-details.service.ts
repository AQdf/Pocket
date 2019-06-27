import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment'
import { BaseService } from './base.service';

const userDetailsApiUrl = environment.baseApiUrl + 'user/details/';

@Injectable({
  providedIn: 'root'
})
export class UserDetailsService extends BaseService {

  constructor(private http: HttpClient) {
    super();
   }

  getUserDetails() {
    return this.http.get(userDetailsApiUrl, this.getDefaultOptions());
  }
}
