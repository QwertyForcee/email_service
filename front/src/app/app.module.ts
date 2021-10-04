import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { AuthComponent } from './components/auth/auth.component';
import {ReactiveFormsModule} from '@angular/forms';
import { ShowApisComponent } from './components/show-apis/show-apis.component';
import { CoinTaskFormComponent } from './components/coin-task-form/coin-task-form.component'
import { environment } from 'src/environments/environment';
import { JwtModule } from '@auth0/angular-jwt';
import { ACCESS_TOKEN_KEY } from './services/auth.service';
import { API_URL } from './app-injection-tokens';
import { CurrencyTaskFormComponent } from './components/currency-task-form/currency-task-form.component';
import { QuoteTaskFormComponent } from './components/quote-task-form/quote-task-form.component';
import { ShowCoinTasksComponent } from './components/show-coin-tasks/show-coin-tasks.component';
import { ShowCurrencyTasksComponent } from './components/show-currency-tasks/show-currency-tasks.component';
import { ShowQuoteTasksComponent } from './components/show-quote-tasks/show-quote-tasks.component';
import { StatsComponent } from './components/stats/stats.component';

export function tokenGetter(){
  return localStorage.getItem(ACCESS_TOKEN_KEY)
}

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    AuthComponent,
    ShowApisComponent,
    CoinTaskFormComponent,
    CurrencyTaskFormComponent,
    QuoteTaskFormComponent,
    ShowCoinTasksComponent,
    ShowCurrencyTasksComponent,
    ShowQuoteTasksComponent,
    StatsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config:{
        tokenGetter,
        allowedDomains:environment.tokenWhiteListedDomains
      }
    })
  ],
  providers: [
    {
      provide: API_URL,
      useValue: environment.authApi
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
