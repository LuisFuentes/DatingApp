import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../app_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register() {
    // On Register button click, send the model to the Auth
    // service which will handle sending the HTTP Request to the API.
    this.authService.register(this.model).subscribe(() => {
      console.log('Registration Successfully');
    }, error => {
      console.log('Error on Registration');
    });
  }

  cancel() {
    // On Cancel button click
    this.cancelRegister.emit();
    console.log('cancelled');
  }

}
