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
import { AssetSettingsComponent } from './components/assets/asset-settings/asset-settings.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AppRoutingModule } from './/app-routing.module';
import { TotalsComponent } from './components/totals/totals.component';
import { BalancesComponent } from './components/balances/balances.component';
import { BalanceListComponent } from './components/balances/balance-list/balance-list.component';
import { BalancesChartComponent } from './components/balances-chart/balances-chart.component';
import { RegistrationComponent } from './components/users/registration/registration.component';
import { LoginComponent } from './components/users/login/login.component';
import { BalanceNoteComponent } from './components/balances/balance-note/balance-note.component';
import { AccountStatementComponent } from './components/assets/account-statement/account-statement.component';
import { ExchangeRatesComponent } from './components/exchange-rates/exchange-rates.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { EffectiveDateComponent } from './components/effective-date/effective-date.component';

@NgModule({
  declarations: [
    AppComponent,
    AssetsComponent,
    AssetSettingsComponent,
    DashboardComponent,
    TotalsComponent,
    BalancesComponent,
    BalanceListComponent,
    BalancesChartComponent,
    RegistrationComponent,
    LoginComponent,
    BalanceNoteComponent,
    AccountStatementComponent,
    ExchangeRatesComponent,
    NavigationComponent,
    EffectiveDateComponent,
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
