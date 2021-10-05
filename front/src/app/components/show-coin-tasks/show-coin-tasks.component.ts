import { Component, OnInit } from '@angular/core';
import { FormControl, FormControlName, FormGroup, Validators } from '@angular/forms';
import { Frequencies } from 'src/app/frequencies';
import { Coin } from 'src/app/models/Coin';
import { CoinTaskService } from 'src/app/services/coin-task.service';

@Component({
  selector: 'app-show-coin-tasks',
  templateUrl: './show-coin-tasks.component.html',
  styleUrls: ['./show-coin-tasks.component.scss']
})
export class ShowCoinTasksComponent implements OnInit {

  constructor(private coinService:CoinTaskService) {
    this.EditForm=new FormGroup({
      "Id":new FormControl(),
      "Name":new FormControl("",Validators.required),
      "Description":new FormControl("",Validators.required),
      "ExecutionMoment":new FormControl(null,Validators.required),
      "Frequency":new FormControl(null,Validators.required),
      "CoinId":new FormControl(null,Validators.required),
    })
   }
  tasks:any[]=[]
  coins:Coin[]=[]
  frequencies:any=[]
  isEditMode=false

  EditForm:FormGroup;
  ngOnInit(): void {
    this.coinService.getCoins().subscribe(
      coins=> {
        this.coins = coins;
      }
    )
    this.coinService.getUserCoinTasks().subscribe(result=>{
      this.tasks=result
      console.log(result)
    })
    this.frequencies = Frequencies
  }

  editTask(id:number){
    this.EditForm.patchValue({Id:id})
    this.isEditMode = true
  }

  deleteTask(id:number){
    this.coinService.deleteTask(id).subscribe()
  }
  submit(){
    let frequency = this.frequencies[this.EditForm.get('Frequency')?.value][1]
    this.EditForm.patchValue({'Frequency':frequency})

    this.coinService.putTask(this.EditForm.value).subscribe()
  }

}
