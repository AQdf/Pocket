import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { DashboardComponent } from './components/dashboard/dashboard.component'
import { BalancesComponent } from './components/balances/balances.component'
import { AssetsComponent } from './components/assets/assets.component'
import { InventoryComponent } from './components/inventory/inventory.component'
import { RegistrationComponent } from './components/users/registration/registration.component'
import { LoginComponent } from './components/users/login/login.component'
import { AssetSettingsComponent } from './components/assets/asset-settings/asset-settings.component';
import { AuthGuard } from './auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'balances', component: BalancesComponent, canActivate: [AuthGuard] },
  { path: 'assets', component: AssetsComponent, canActivate: [AuthGuard] },
  { path: 'asset-settings/:id', component: AssetSettingsComponent, canActivate: [AuthGuard] },
  { path: 'inventory', component: InventoryComponent, canActivate: [AuthGuard] },
  { path: 'register', component: RegistrationComponent },
  { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ], 
  exports: [ RouterModule ]
})
export class AppRoutingModule { }
