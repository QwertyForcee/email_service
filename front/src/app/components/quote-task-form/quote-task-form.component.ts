import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { QuoteTaskService } from 'src/app/services/quote-task.service';

@Component({
  selector: 'app-quote-task-form',
  templateUrl: './quote-task-form.component.html',
  styleUrls: ['./quote-task-form.component.scss']
})
export class QuoteTaskFormComponent implements OnInit {

  constructor(private quoteTaskService:QuoteTaskService) {
    this.quoteTaskForm = new FormGroup({
      "Name":new FormControl("",Validators.required),
      "Description":new FormControl("",Validators.required),
      "ExecutionMoment":new FormControl(null,Validators.required),
      "Frequency":new FormControl(null,Validators.required),
      "Lang":new FormControl("en",Validators.required),
    })
  }
  quoteTaskForm:FormGroup;
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

  }
  submit(){
    this.quoteTaskService.postTask(this.quoteTaskForm.value).subscribe(r=>console.log(r))
  }

}
