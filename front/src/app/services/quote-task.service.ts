import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { quoteTask } from '../models/quoteTask';

@Injectable({
  providedIn: 'root'
})
export class QuoteTaskService {

  private url = "https://localhost:5001/api/Quotes/"
  constructor(private http:HttpClient) { }

  getQuotes():Observable<any>{
    return this.http.get(this.url + 'tasks')
  }
  postTask(taskForm:any):Observable<any>{
    let task = {
      name:taskForm.Name,
      description: taskForm.Description,
      coinId:taskForm.CoinId,
      cronExpression:this.ToCronExpr(taskForm.Frequency,taskForm.ExecutionMoment) ,
      lang:taskForm.Lang
    }
    console.log(task)
    return this.http.post(this.url+'tasks',task);
  }
  putTask(task:quoteTask):Observable<any>{
    return this.http.put(this.url+'tasks',task);
  }

  deleteTask(id:number):Observable<any>{
    return this.http.delete(this.url+'tasks/'+id)
  }
  getQuoteById(id:number):Observable<any>{
    return this.http.get(this.url+'tasks/'+id)
  }
  getUserQuoteTasks():Observable<any>{
    return this.http.get(this.url+'tasks/user')
  };
  private ToCronExpr(Frequency:string,ExecutionMoment:string){
    let minutes:number =  +ExecutionMoment.substring(3)
    let hours:number = +ExecutionMoment.substring(0,2)

    let exp = ['*','*','*','*','*']
    exp[0] = minutes.toString()
    exp[1] = hours.toString()
    if (Frequency!=='1')
      exp[2] = exp[2]+'/'+Frequency
    return exp.join(' ')
  }
}
