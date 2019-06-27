import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, finalize, map, pluck } from 'rxjs/operators';

import { UserService } from '../../../services/user.service';
import { UserRegistration } from'../../../models/user-registration.model';
import { ResponseError } from'../../../models/response-error.model';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  errors: ResponseError[];  
  isRequesting: boolean;
  submitted: boolean = false;

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit() {
  }

  registerUser({ value, valid } : { value: UserRegistration, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    if (valid) {
      this.userService
      .register(value).pipe(
          finalize(() => this.isRequesting = false))
      .subscribe(result => {
        if(result) {
          this.router.navigate(['/login'], {queryParams: { brandNew: true, email: value.email }});
        }
      }, (errors: ResponseError[]) => this.errors = errors)
    }      
  }

}
