import { Component, Inject, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DateTimeTest, DateTimeWithTzInfo } from "./DateTimeTest";
import flatpickr from "flatpickr";
import {} from 'flatpickr/dist/l10n/'

@Component({
  selector: 'app-date-time-testing',
  templateUrl: './date-time-testing.component.html'
})
export class DateTimeTestingComponent implements AfterViewInit {
  dateTimes: DateTimeTest[];
  @ViewChild("datepicker", { static: false }) dateInputRef: ElementRef;
  get timeZone(): string { console.log('dirty check'); return Intl.DateTimeFormat().resolvedOptions().timeZone; }
  get pickedDate(): Date {
    if (this.flatpickrInstance == undefined) return undefined;
    if (this.flatpickrInstance.selectedDates == undefined) return undefined;
    if (this.flatpickrInstance.selectedDates.length === 0) return undefined;
    return this.flatpickrInstance.selectedDates[0];
  }

  private dateTimeWithTzInfo(date: Date): DateTimeWithTzInfo {
    return {
      timestamp: date,
      timeZoneId: this.timeZone,
      timeZoneOffset: this.getISOUTCOffset(date)
    };
  }

  getISOUTCOffset(date: Date): string {
    const offset = -date.getTimezoneOffset();
    if (offset === 0) return 'Z';
    const sign = offset < 0 ? '-' : '+';
    const hours = Math.abs(Math.floor(offset / 60));
    const minutes = Math.abs(offset % 60);
    return `${sign}${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
  }

  private flatpickrInstance: flatpickr.Instance;

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) {
    http.get<DateTimeTest[]>(`${this.baseUrl}dateTimeTesting/getDates`).subscribe(result => {
      this.dateTimes = result;
    }, error => console.error(error));
  }

  ngAfterViewInit() {
    this.initialiseDatepicker();
  }

  saveFromDatepicker() {
    this.http.post<DateTimeTest>(`${this.baseUrl}dateTimeTesting/setDate`, this.dateTimeWithTzInfo(this.pickedDate)).subscribe(result => {
      this.dateTimes.push(result);
    }, error => console.error(error));
  }

  saveNowFromClient() {
    this.http.post<DateTimeTest>(`${this.baseUrl}dateTimeTesting/setDate`, this.dateTimeWithTzInfo(new Date())).subscribe(result => {
      this.dateTimes.push(result);
    }, error => console.error(error));
  }

  saveUtcNowFromServer() {
    this.http.get<DateTimeTest>(`${this.baseUrl}dateTimeTesting/setNow`).subscribe(result => {
      this.dateTimes.push(result);
    }, error => console.error(error));
  }

  initialiseDatepicker() {
    this.flatpickrInstance = flatpickr(this.dateInputRef.nativeElement, {
      onChange: () => console.log('onChange'),
      onOpen: () => console.log('onOpen'),
      onClose: () => console.log('onClose'),
      onMonthChange: () => console.log('onMonthChange'),
      onYearChange: () => console.log('onYearChange'),
      onReady: () => console.log('onReady'),
      onValueUpdate: () => console.log('onValueUpdate'),
      onDayCreate: () => console.log('onDayCreate')
  });
    console.log('flatpickr default date:', this.flatpickrInstance.config.defaultDate);
    console.log('flatpickr default hour:', this.flatpickrInstance.config.defaultHour);
    console.log('flatpickr default minute:', this.flatpickrInstance.config.defaultMinute);
    console.log('flatpickr default second:', this.flatpickrInstance.config.defaultSeconds);
    console.log('flatpickr auto-fill default time:', this.flatpickrInstance.config.autoFillDefaultTime);
  }
}
