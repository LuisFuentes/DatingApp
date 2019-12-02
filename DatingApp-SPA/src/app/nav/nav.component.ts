import { Component, OnInit } from '@angular/core';
import { AuthService } from '../app_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  // Inject the Auth service into the constructor to use
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }


  login() {
    // Function handles when the login button is clicked.

    this.authService.login(this.model).subscribe(
      next => {console.log('Logged in successfully'); },
      error => {console.log('Error logging in'); }
    );

    // console.log(this.model);
  }

  loggedIn() {
    // Function checks if user is logged in.
    // Fetch the JWT from the browser storage,
    // and return true if there's a token, false if not.
    return !!(localStorage.getItem('token'));
  }

  logout() {
    // Function handles logging out the current user.
    // Removes the JWT from the browser storage.
    localStorage.removeItem('token');
    console.log('Logged out');
  }

}
