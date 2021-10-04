import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { currencyTask } from '../models/currencyTask';

@Injectable({
  providedIn: 'root'
})
export class CurrencyTaskService {

  private url = "https://localhost:5001/api/Currency/"
  constructor(private http:HttpClient) { }

  getCurrencyList():Observable<any>{
    return this.http.get(this.url+"list")
  }

  getCurrencies():Observable<any>{
    return this.http.get(this.url+'tasks')
  }

  postTask(taskForm:any):Observable<any>{
    let task = {
      name:taskForm.Name,
      description: taskForm.Description,
      cronExpression:this.ToCronExpr(taskForm.Frequency,taskForm.ExecutionMoment) ,
      from:taskForm.From,
      to:taskForm.To,
      count:taskForm.Count
    }
    console.log(task)
    return this.http.post(this.url+'tasks',task);
  }

  putTask(task:currencyTask):Observable<any>{
    return this.http.put(this.url+'tasks',task);
  }

  deleteTask(id:number):Observable<any>{
    return this.http.delete(this.url+'tasks/'+id)
  }
  getCurrencyById(id:number):Observable<any>{
    return this.http.get(this.url+'tasks/'+id)
  }
  getUserCurrencyTasks():Observable<any>{
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
