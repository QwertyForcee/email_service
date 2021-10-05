import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { quoteTask } from '../models/quoteTask';
import { ToCronExpr } from '../frequencies';

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
      cronExpression:ToCronExpr(taskForm.Frequency,taskForm.ExecutionMoment) ,
      lang:taskForm.Lang
    }
    console.log(task)
    return this.http.post(this.url+'tasks',task);
  }
  putTask(taskForm:any):Observable<any>{
    let task = {
      name:taskForm.Name,
      description: taskForm.Description,
      coinId:taskForm.CoinId,
      cronExpression:ToCronExpr(taskForm.Frequency,taskForm.ExecutionMoment) ,
      lang:taskForm.Lang
    }
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
}
