import { Component, OnInit } from '@angular/core';
import { from } from 'rxjs';
import { distinct, map, toArray, filter } from 'rxjs/operators';

import { BalanceService } from '../../services/balance.service'

@Component({
  selector: 'app-effective-date',
  templateUrl: './effective-date.component.html',
  styleUrls: ['./effective-date.component.css']
})
export class EffectiveDateComponent implements OnInit {

  effectiveDates: Date[];
  years: number[];
  months: number[];
  days: number[];

  selectedYear: number;
  selectedMonth: number;
  selectedDay: number;
  monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

  constructor(private balanceService: BalanceService) { }

  ngOnInit() {
    this.getEffectiveDates();
  }

  getEffectiveDates() {
    this.balanceService.getEffectiveDates().subscribe((effectiveDates: Date[]) => {
      this.effectiveDates = effectiveDates;

      from(this.effectiveDates).pipe(
        map(date => new Date(date).getFullYear()),
        distinct(),
        toArray(),
      ).subscribe(years => {
        this.years = years.sort((a,b) => b - a);
      }); 
    });
  }

  selectYear(year: number) {
    this.selectedYear = year;

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
  }

  clear() {
    this.selectedYear = null;
    this.selectedMonth = null;
    this.selectedDay = null;
  }
}
