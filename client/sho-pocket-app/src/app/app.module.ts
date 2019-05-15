import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpModule, XHRBackend } from '@angular/http'
import { HttpClientModule } from '@angular/common/http';
import { FormsModule} from '@angular/forms'
import { ToastrModule } from 'ngx-toastr';
import { ChartModule } from 'angular-highcharts';

import { AuthXHRBackend } from './auth-xhr-backend';
import { AuthGuard } from './auth.guard';

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
import { BalancesChartComponent } from './components/balances-chart/balances-chart.component';
import { RegistrationComponent } from './components/users/registration/registration.component';
import { LoginComponent } from './components/users/login/login.component';

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
    BalanceListComponent,
    BalancesChartComponent,
    RegistrationComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpModule,
    HttpClientModule,
    FormsModule,
    ToastrModule.forRoot(),
    AppRoutingModule,
    ChartModule
  ],
  providers: [AuthGuard, { 
    provide: XHRBackend, 
    useClass: AuthXHRBackend
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
