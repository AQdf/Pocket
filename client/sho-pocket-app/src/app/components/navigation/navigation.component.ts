import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {

  status: boolean = false;
  subscription: Subscription;
  collapsed: boolean = true;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.subscription = this.userService.authNavStatus$.subscribe(status => this.status = status);
    this.collapsed = JSON.parse(localStorage.getItem('sidebar-collapsed'));
  }
 
  ngOnDestroy() {
    // prevent memory leak when component is destroyed
    this.subscription.unsubscribe();
  }

  logout() {
    this.userService.logout();
  }

  toggleSidebar() {
    this.collapsed = !this.collapsed;
    localStorage.setItem('sidebar-collapsed', this.collapsed.toString())
  }
}
