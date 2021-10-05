import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Frequencies } from 'src/app/frequencies';
import { CurrencyTaskService } from 'src/app/services/currency-task.service';

@Component({
  selector: 'app-show-currency-tasks',
  templateUrl: './show-currency-tasks.component.html',
  styleUrls: ['./show-currency-tasks.component.scss']
})
export class ShowCurrencyTasksComponent implements OnInit {

  constructor(private currencyService:CurrencyTaskService) {
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
  currencies:any=[]
  isEditMode=false
  EditForm:FormGroup
  frequencies:any=[]

  ngOnInit(): void {
    this.currencyService.getCurrencyList().subscribe(list=>this.currencies=list)
    this.currencyService.getUserCurrencyTasks().subscribe(r=>this.tasks=r)
    this.frequencies = Frequencies
  }


  editTask(id:number){
    this.EditForm.patchValue({Id:id})
    this.isEditMode = true
  }

  deleteTask(id:number){
    this.currencyService.deleteTask(id).subscribe()
  }
  submit(){
    let frequency = this.frequencies[this.EditForm.get('Frequency')?.value][1]
    this.EditForm.patchValue({'Frequency':frequency})

    this.currencyService.putTask(this.EditForm.value).subscribe()
  }
}
