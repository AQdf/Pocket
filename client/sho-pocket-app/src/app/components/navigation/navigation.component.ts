import { Component, OnInit } from '@angular/core';

import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {

  collapsed: boolean = true;
  userEmail: string;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.collapsed = JSON.parse(localStorage.getItem('sidebar_collapsed'));
    this.userEmail = localStorage.getItem("user_email");
  }

  logout() {
    this.userService.logout();
  }

  toggleSidebar() {
    this.collapsed = !this.collapsed;
    localStorage.setItem('sidebar_collapsed', this.collapsed.toString())
  }
}
