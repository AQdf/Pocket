import { Component, OnInit } from '@angular/core';

import { SummaryService } from './shared/summary.service'

@Component({
  selector: 'app-summaries',
  templateUrl: './summaries.component.html',
  styleUrls: ['./summaries.component.css'],
  providers: [SummaryService]
})
export class SummariesComponent implements OnInit {

  constructor(private summaryService: SummaryService) { }

  ngOnInit() {
  }

}
