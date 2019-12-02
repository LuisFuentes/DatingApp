import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth/';

  constructor(private http: HttpClient) {
  }

  login(model: any) {
    // Function sends a HTTP Post to the login API with
    // the provided model containing username and password

    // The API returns a JWT to be used when authenticated.
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((response: any) => {

          if (response) {
            // There's a response from the API, store the JWT
            localStorage.setItem('token', response.token);
          }
        })
      );
  }

  register(model: any) {
    // Function sends a HTTP Post to the Register API
    // with the provided model containing
    // XXX AND YYY

    return this.http.post(this.baseUrl + 'register', model)
      .pipe(
        map((response: any) => {


        })
      );

  }

}
