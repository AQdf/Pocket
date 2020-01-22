import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule} from '@angular/forms'
import { ToastrModule } from 'ngx-toastr';
import { ChartModule } from 'angular-highcharts';

import { AuthGuard } from './auth.guard';
import { HttpErrorInterceptor } from './common/http-error.interceptor';
import { AuthHeaderInterceptor } from './common/auth-header.interceptor';

import { AppComponent } from './app.component';
import { AssetsComponent } from './components/assets/assets.component';
import { AssetComponent } from './components/assets/asset/asset.component';
import { AssetListComponent } from './components/assets/asset-list/asset-list.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AppRoutingModule } from './/app-routing.module';
import { TotalsComponent } from './components/totals/totals.component';
import { BalancesComponent } from './components/balances/balances.component';
import { BalanceListComponent } from './components/balances/balance-list/balance-list.component';
import { BalancesChartComponent } from './components/balances-chart/balances-chart.component';
import { RegistrationComponent } from './components/users/registration/registration.component';
import { LoginComponent } from './components/users/login/login.component';
import { InventoryComponent } from './components/inventory/inventory.component';
import { InventoryItemsComponent } from './components/inventory/inventory-items/inventory-items.component';
import { BalanceNoteComponent } from './components/balances/balance-note/balance-note.component';
import { AccountStatementComponent } from './components/assets/account-statement/account-statement.component';
import { ExchangeRatesComponent } from './components/exchange-rates/exchange-rates.component';
import { NavigationComponent } from './components/navigation/navigation.component';

@NgModule({
  declarations: [
    AppComponent,
    AssetsComponent,
    AssetComponent,
    AssetListComponent,
    DashboardComponent,
    TotalsComponent,
    BalancesComponent,
    BalanceListComponent,
    BalancesChartComponent,
    RegistrationComponent,
    LoginComponent,
    InventoryComponent,
    InventoryItemsComponent,
    BalanceNoteComponent,
    AccountStatementComponent,
    ExchangeRatesComponent,
    NavigationComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ToastrModule.forRoot(),
    AppRoutingModule,
    ChartModule
  ],
  providers: [
    AuthGuard,
    { 
      provide: HTTP_INTERCEPTORS, 
      useClass: AuthHeaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
