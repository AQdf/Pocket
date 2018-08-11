import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { AssetsComponent } from './assets/assets.component'
import { AssetsHistoryComponent } from './assets-history/assets-history.component'

const routes: Routes = [
  { path: '', redirectTo: '/assets', pathMatch: 'full' },
  { path: 'assets', component: AssetsComponent },
  { path: 'assets-history', component: AssetsHistoryComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [ RouterModule ]
})
export class AppRoutingModule { }
