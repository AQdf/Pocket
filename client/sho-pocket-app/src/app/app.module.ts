import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http'
import { HttpClientModule } from '@angular/common/http';
import { FormsModule} from '@angular/forms'
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { AssetsComponent } from './components/assets/assets.component';
import { AssetComponent } from './components/assets/asset/asset.component';
import { AssetListComponent } from './components/assets/asset-list/asset-list.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AppRoutingModule } from './/app-routing.module';
import { SnapshotComponent } from './components/snapshot/snapshot.component';
import { TotalsComponent } from './components/totals/totals.component';
import { BalancesComponent } from './components/balances/balances.component';
import { BalanceComponent } from './components/balances/balance/balance.component';
import { BalanceListComponent } from './components/balances/balance-list/balance-list.component';

@NgModule({
  declarations: [
    AppComponent,
    AssetsComponent,
    AssetComponent,
    AssetListComponent,
    DashboardComponent,
    SnapshotComponent,
    TotalsComponent,
    BalancesComponent,
    BalanceComponent,
    BalanceListComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpModule,
    HttpClientModule,
    FormsModule,
    ToastrModule.forRoot(),
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
