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
import { AppRoutingModule } from './/app-routing.module';
import { SnapshotComponent } from './snapshot/snapshot.component';
import { TotalsComponent } from './totals/totals.component';

@NgModule({
  declarations: [
    AppComponent,
    AssetsComponent,
    AssetComponent,
    AssetListComponent,
    DashboardComponent,
    SnapshotComponent,
    TotalsComponent
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
