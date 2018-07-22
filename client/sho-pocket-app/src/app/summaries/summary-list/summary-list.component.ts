import { Component, OnInit } from '@angular/core';

import { SummaryService } from '../shared/summary.service'
import { Summary } from '../shared/summary.model';   

@Component({
  selector: 'app-summary-list',
  templateUrl: './summary-list.component.html',
  styleUrls: ['./summary-list.component.css']
})
export class SummaryListComponent implements OnInit {

  constructor(private summaryService: SummaryService) { }

  ngOnInit() {
    var yo = this.summaryService.getPeriodSummaryList();
  }

}
