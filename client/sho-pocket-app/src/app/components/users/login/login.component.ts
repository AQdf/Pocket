import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';

import { UserService } from '../../../services/user.service';
import { UserLogin } from'../../../models/user-login.model';
import { ResponseError } from 'src/app/models/response-error.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  private subscription: Subscription;

  brandNew: boolean;
  errors: ResponseError[];
  isRequesting: boolean;
  submitted: boolean = false;
  credentials: UserLogin = { email: '', password: '' };

  constructor(private userService: UserService, private router: Router,private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    // subscribe to router event
    this.subscription = this.activatedRoute.queryParams.subscribe(
      (param: any) => {
         this.brandNew = param['brandNew'];   
         this.credentials.email = param['email'];         
      });      
  }

  ngOnDestroy() {
    // prevent memory leak by unsubscribing
    this.subscription.unsubscribe();
  }

  login({ value, valid }: { value: UserLogin, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    if (valid) {
      this.userService
        .login(value.email, value.password).pipe(
          finalize(() => this.isRequesting = false)
        )
        .subscribe(result => {
          if (result) {
             this.router.navigate(['/dashboard']);
          }
        }, (errors: ResponseError[]) => this.errors = errors);
    }
  }

}
