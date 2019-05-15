import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserService } from '../../../services/user.service';
import { UserRegistration } from'../../../models/user-registration.model'

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  errors: string;  
  isRequesting: boolean;
  submitted: boolean = false;

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit() {
  }

  registerUser({ value, valid } : { value: UserRegistration, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';
    if (valid)
    {
      this.userService
        .register(value.email, value.password, value.firstName, value.lastName, value.location)
        //.finally(() => this.isRequesting = false)
        .subscribe(result  => {
          if(result) {
            this.router.navigate(['/login'], {queryParams: { brandNew: true,email:value.email }});
          }
        }, errors =>  this.errors = errors);
    }      
  }

}
