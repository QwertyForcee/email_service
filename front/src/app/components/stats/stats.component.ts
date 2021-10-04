import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { CoinTaskService } from 'src/app/services/coin-task.service';
import { CurrencyTaskService } from 'src/app/services/currency-task.service';
import { QuoteTaskService } from 'src/app/services/quote-task.service';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.scss']
})
export class StatsComponent implements OnInit {

  constructor(private auth:AuthService,private coinService:CoinTaskService,
              private currencyService:CurrencyTaskService,private quoteService:QuoteTaskService ) { }

  users_count=0
  currencies_count=0
  quotes_count=0
  coins_count=0
  ngOnInit(): void {
    this.auth.getAllUsers().subscribe(users=>this.users_count=users.length)
    this.coinService.getCoinTasks().subscribe(coins=>this.coins_count=coins.length)
    this.currencyService.getCurrencies().subscribe(currencies=> this.currencies_count=currencies.length)
    this.quoteService.getQuotes().subscribe(quotes=>this.quotes_count=quotes.length)
  }

}
