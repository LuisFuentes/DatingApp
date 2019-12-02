import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;

  // Construct a http client obj
  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  registerToggle() {
    // Function toggles the register button to hide/show fields
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(regMode: boolean) {
    // Function handles when the Cancel button is clicked on
    // the register form
    this.registerMode = regMode;
  }
}
