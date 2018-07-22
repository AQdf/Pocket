import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http'
import { FormsModule} from '@angular/forms'

import { AppComponent } from './app.component';
import { SummaryComponent } from './summaries/summary/summary.component';
import { SummaryListComponent } from './summaries/summary-list/summary-list.component';
import { SummariesComponent } from './summaries/summaries.component';

@NgModule({
  declarations: [
    AppComponent,
    SummaryComponent,
    SummaryListComponent,
    SummariesComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
