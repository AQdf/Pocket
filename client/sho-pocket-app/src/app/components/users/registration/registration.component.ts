import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';

import { UserService } from '../../../services/user.service';
import { UserRegistration } from'../../../models/user-registration.model';
import { ResponseError } from'../../../models/response-error.model';
import { CurrencyService } from '../../../services/currency.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  errors: ResponseError[];
  currenciesList: string[];
  isRequesting: boolean;
  submitted: boolean = false;

  constructor(private userService: UserService, private currencyService: CurrencyService, private router: Router) { }

  ngOnInit() {
    this.getCurrenciesList();
  }

  registerUser({ value, valid } : { value: UserRegistration, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    if (valid) {
      this.userService
      .register(value).pipe(finalize(() => this.isRequesting = false))
      .subscribe(result => {
        if(result) {
          this.router.navigate(['/login'], {queryParams: { brandNew: true, email: value.email }});
        }
      }, (errors: ResponseError[]) => this.errors = errors)
    }
  }

  getCurrenciesList() {
    this.currencyService.getCurrenciesList().subscribe((response: string[]) => {
      this.currenciesList = response;
    }, (errors: ResponseError[]) => this.errors = errors);
  }
}
