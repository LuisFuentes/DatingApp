import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-value',
    templateUrl: './value.component.html',
    styleUrls: ['./value.component.css']
})
/** value component*/
export class ValueComponent {
  /** value ctor */

  values: any;

  // Construct a http client obj
  constructor(private http: HttpClient) { }

  ngOnInit() {
    // Once init-ed, get the values
    this.getValues();
  }

  getValues() {
    // Make a HTTP GET Request to get the values
    this.http.get("http://localhost:5000/api/values").subscribe(response => {
      // Set the values from the resp
      this.values = response;
    },
      error => { console.log(error); }
    );
  }
}
