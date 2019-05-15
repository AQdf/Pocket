import { Component } from '@angular/core';
import { Subscription } from 'rxjs';

import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Pocket';

  status: boolean = false;
  subscription:Subscription;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.subscription = this.userService.authNavStatus$.subscribe(status => this.status = status);
  }
 
  ngOnDestroy() {
    // prevent memory leak when component is destroyed
    this.subscription.unsubscribe();
  }

  logout() {
    this.userService.logout();
  }
}
