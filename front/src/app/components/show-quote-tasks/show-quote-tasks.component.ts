import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Frequencies } from 'src/app/frequencies';
import { QuoteTaskService } from 'src/app/services/quote-task.service';

@Component({
  selector: 'app-show-quote-tasks',
  templateUrl: './show-quote-tasks.component.html',
  styleUrls: ['./show-quote-tasks.component.scss']
})
export class ShowQuoteTasksComponent implements OnInit {

  EditForm:FormGroup
  isEditMode=false
  tasks:any[]=[]
  constructor(private quoteService:QuoteTaskService) {
    this.EditForm=new FormGroup({
      "Id":new FormControl(),
      "Name":new FormControl("",Validators.required),
      "Description":new FormControl("",Validators.required),
      "ExecutionMoment":new FormControl(null,Validators.required),
      "Frequency":new FormControl(null,Validators.required),
      "CoinId":new FormControl(null,Validators.required),
    })
   }
   frequencies:any=[]
   languages=[
    {code: 'en' , full:'English'},
    {code: 'es',full:'Spanish'},
    {code: 'pt',full:'Portuguese'},
    {code: 'it',full:'Italian'},
    {code: 'de',full:'German'},
    {code: 'fr',full:'French'},
    {code: 'ru',full:'Russian'},
  ]
  ngOnInit(): void {
    this.quoteService.getUserQuoteTasks().subscribe(r=>this.tasks=r)
    this.frequencies = Frequencies
  }

  editTask(id:number){
    this.EditForm.patchValue({Id:id})
    this.isEditMode = true
  }

  deleteTask(id:number){
    this.quoteService.deleteTask(id).subscribe()
  }
  submit(){
    let frequency = this.frequencies[this.EditForm.get('Frequency')?.value][1]
    this.EditForm.patchValue({'Frequency':frequency})

    this.quoteService.putTask(this.EditForm.value).subscribe()
  }
}
