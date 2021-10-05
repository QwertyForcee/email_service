import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Coin } from 'src/app/models/Coin';
import { coinTask } from 'src/app/models/coinTask';
import { CoinTaskService } from 'src/app/services/coin-task.service';
import { Frequencies } from 'src/app/frequencies';

@Component({
  selector: 'app-coin-task-form',
  templateUrl: './coin-task-form.component.html',
  styleUrls: ['./coin-task-form.component.scss']
})
export class CoinTaskFormComponent implements OnInit {

  constructor(private coinTaskService:CoinTaskService) {
    this.coinTaskForm = new FormGroup({
      "Name":new FormControl("",Validators.required),
      "Description":new FormControl("",Validators.required),
      "ExecutionMoment":new FormControl(null,Validators.required),
      "Frequency":new FormControl(null,Validators.required),
      "CoinId":new FormControl(null,Validators.required),
    })
  }
  coinTaskForm:FormGroup;
  coins:Coin[] = [];
  frequencies:any=[]
  ngOnInit(): void {

    this.coinTaskService.getCoins().subscribe(
      coins=> {
        this.coins = coins;
        console.log(coins);
      }
    )
    this.frequencies = Frequencies
  }
  submit(){
    let frequency = this.frequencies[this.coinTaskForm.get('Frequency')?.value][1]
    this.coinTaskForm.patchValue({'Frequency':frequency})

    this.coinTaskService.postTask(this.coinTaskForm.value).subscribe()
  }

}
