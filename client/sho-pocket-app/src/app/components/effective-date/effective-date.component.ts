import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { from, Subscription } from 'rxjs';
import { distinct, map, toArray, filter } from 'rxjs/operators';

import { EffectiveDateService } from '../../services/effective-date.service'

@Component({
  selector: 'app-effective-date',
  templateUrl: './effective-date.component.html',
  styleUrls: ['./effective-date.component.css']
})
export class EffectiveDateComponent implements OnInit {

  @Output() effectiveDateEvent = new EventEmitter<string>();
  subscription: Subscription;
  
  selectedEffectiveDate: string;
  effectiveDates: Date[];
  years: number[];
  months: number[];
  days: number[];

  selectingYear: boolean;
  selectingMonth: boolean;
  selectingDay: boolean;

  selectedYear: number;
  selectedMonth: number;
  selectedDay: number;
  monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

  constructor(private effectiveDateService: EffectiveDateService) {
    this.subscription = effectiveDateService.effectiveDatesReload$.subscribe(() => this.reload());
  }

  ngOnInit() {
    this.reload();
  }

  reload() {
    this.effectiveDateService.getEffectiveDates().subscribe((effectiveDates: Date[]) => {
      this.effectiveDates = effectiveDates;
      this.setToday();
    });
  }

  setToday() {
    if (this.effectiveDates.length > 0) {
      let latestDate = new Date(this.effectiveDates[0]);
      this.selectedEffectiveDate = latestDate.toISOString()

      this.setYears();
      this.selectYear(latestDate.getFullYear());
      this.selectMonth(latestDate.getMonth());
      this.selectDay(latestDate.getDate());
    } else {
      this.years = null;
      this.months = null;
      this.days = null;
      this.selectedYear = null;
      this.selectedMonth = null;
      this.selectedDay = null;
      this.selectedEffectiveDate = null;
      this.effectiveDateEvent.emit(this.selectedEffectiveDate);
    }
  }

  setYears() {
    from(this.effectiveDates).pipe(
      map(date => new Date(date).getFullYear()),
      distinct(),
      toArray(),
    ).subscribe(years => {
      this.years = years.sort((a,b) => b - a);
    }); 
  }

  selectYear(year: number) {
    this.selectedYear = year;
    this.selectedMonth = null;
    this.selectedDay = null;
    this.selectingMonth = true;

    from(this.effectiveDates).pipe(
      filter(date => new Date(date).getFullYear() === this.selectedYear),
      map(date => new Date(date).getMonth()),
      distinct(),
      toArray(),
    ).subscribe(months => {
      this.months = months.sort((a,b) => a - b);
    }); 
  }

  selectMonth(month: number) {
    this.selectedMonth = month;
    this.selectedDay = null;
    this.selectingDay = true;

    from(this.effectiveDates).pipe(
      filter(dateStr => {
        let date = new Date(dateStr);
        return date.getFullYear() === this.selectedYear && date.getMonth() === this.selectedMonth;
      }),
      map(date => new Date(date).getDate()),
      distinct(),
      toArray(),
    ).subscribe(days => {
      this.days = days.sort((a,b) => a - b);
    }); 
  }

  selectDay(day: number) {
    this.selectedDay = day;
    this.selectingYear = false;
    this.selectingMonth = false;
    this.selectingDay = false;
    let date = new Date(Date.UTC(this.selectedYear, this.selectedMonth, this.selectedDay));

    this.selectedEffectiveDate = date.toISOString();
    this.effectiveDateEvent.emit(this.selectedEffectiveDate);
  }

  onYearClick() {
    this.selectingYear = !this.selectingYear;
    this.selectingMonth = false;
    this.selectingDay = false;
  }

  onMonthClick() {
    this.selectingMonth = !this.selectingMonth;
    this.selectingYear = false;
    this.selectingDay = false;
  }

  onDayClick() {
    this.selectingDay = !this.selectingDay;
    this.selectingYear = false;
    this.selectingMonth = false;
  }
}
