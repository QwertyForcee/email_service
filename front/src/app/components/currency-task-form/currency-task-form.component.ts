import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Frequencies } from 'src/app/frequencies';
import { CurrencyTaskService } from 'src/app/services/currency-task.service';

@Component({
  selector: 'app-currency-task-form',
  templateUrl: './currency-task-form.component.html',
  styleUrls: ['./currency-task-form.component.scss']
})
export class CurrencyTaskFormComponent implements OnInit {

  currencyTaskForm:FormGroup
  currencies=[]
  frequencies:any=[]
  constructor(private currencyTaskService:CurrencyTaskService) {
    this.currencyTaskForm = new FormGroup({
      "Name":new FormControl("",Validators.required),
      "Description":new FormControl("",Validators.required),
      "ExecutionMoment":new FormControl(null,Validators.required),
      "Frequency":new FormControl(null,Validators.required),
      "From":new FormControl("USD",Validators.required),
      "To":new FormControl("RUB",Validators.required),
      "Count":new FormControl(1)
    })
  }
  ngOnInit(): void {
    this.currencyTaskService.getCurrencyList().subscribe(list=>this.currencies=list)
    this.frequencies=Frequencies
  }
  submit(){
    let frequency = this.frequencies[this.currencyTaskForm.get('Frequency')?.value][1]
    this.currencyTaskForm.patchValue({'Frequency':frequency})

    this.currencyTaskService.postTask(this.currencyTaskForm.value).subscribe()
  }

}
