import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './components/auth/auth.component';
import { CoinTaskFormComponent } from './components/coin-task-form/coin-task-form.component';
import { CurrencyTaskFormComponent } from './components/currency-task-form/currency-task-form.component';
import { HomeComponent } from './components/home/home.component';
import { QuoteTaskFormComponent } from './components/quote-task-form/quote-task-form.component';
import { ShowApisComponent } from './components/show-apis/show-apis.component';
import { ShowCoinTasksComponent } from './components/show-coin-tasks/show-coin-tasks.component';
import { ShowCurrencyTasksComponent } from './components/show-currency-tasks/show-currency-tasks.component';
import { ShowQuoteTasksComponent } from './components/show-quote-tasks/show-quote-tasks.component';
import { StatsComponent } from './components/stats/stats.component';
import { AdminGuard } from './guards/admin.guard';
import { AuthGuard } from './guards/auth.guard';

const taskRoutes:Routes=[
  {path:'coins',component:ShowCoinTasksComponent},
  {path:'currencies',component:ShowCurrencyTasksComponent},
  {path:'quotes',component:ShowQuoteTasksComponent}
]
const newRoutes:Routes=[
  {path:'coins',component:CoinTaskFormComponent},
  {path:'currencies',component:CurrencyTaskFormComponent},
  {path:'quotes',component:QuoteTaskFormComponent},
]
const routes: Routes = [
  {path:'',component:HomeComponent},
  {path:'auth',component:AuthComponent},
  {path:'new',component:ShowApisComponent,children:newRoutes,canActivate:[AuthGuard]},
  {path:'tasks',component:ShowApisComponent,children: taskRoutes,canActivate:[AuthGuard]},
  {path:'stats',component:StatsComponent,canActivate:[AdminGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
