import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DateTimeTest } from "./DateTimeTest";

@Component({
  selector: 'app-date-time-testing',
  templateUrl: './date-time-testing.component.html'
})
export class DateTimeTestingComponent {
  public dateTimes: DateTimeTest[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    http.get<DateTimeTest[]>(baseUrl + 'dateTimeTesting/getDates').subscribe(result => {
      this.dateTimes = result;
    }, error => console.error(error));
  }

  setNow() {
    this.http.get<DateTimeTest>(this.baseUrl + 'dateTimeTesting/setNow').subscribe(result => {
      this.dateTimes.push(result);
    }, error => console.error(error));
  }
}
