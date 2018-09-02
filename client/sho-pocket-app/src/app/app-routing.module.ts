import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { DashboardComponent } from './components/dashboard/dashboard.component'
import { BalancesComponent } from './components/balances/balances.component'
import { AssetsComponent } from './components/assets/assets.component'

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'balances', component: BalancesComponent },
  { path: 'assets', component: AssetsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [ RouterModule ]
})
export class AppRoutingModule { }