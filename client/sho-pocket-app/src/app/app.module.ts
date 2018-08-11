import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http'
import { FormsModule} from '@angular/forms'
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { AssetsComponent } from './assets/assets.component';
import { AssetComponent } from './assets/asset/asset.component';
import { AssetListComponent } from './assets/asset-list/asset-list.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AssetsHistoryComponent } from './assets-history/assets-history.component';
import { AssetHistoryComponent } from './assets-history/asset-history/asset-history.component';
import { AssetHistoryListComponent } from './assets-history/asset-history-list/asset-history-list.component';
import { AppRoutingModule } from './/app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    AssetsComponent,
    AssetComponent,
    AssetListComponent,
    DashboardComponent,
    AssetsHistoryComponent,
    AssetHistoryComponent,
    AssetHistoryListComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpModule,
    FormsModule,
    ToastrModule.forRoot(),
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
