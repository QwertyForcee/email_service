import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { coinTask } from '../models/coinTask';

@Injectable({
  providedIn: 'root'
})
export class CoinTaskService {
  private url = "https://localhost:5001/api/Coins/"
  constructor(private http:HttpClient) { }

  getCoins():Observable<any>{
    return this.http.get(this.url+'coins')
  }
  getCoinTasks():Observable<any>{
    return this.http.get(this.url+'tasks')
  }

  postTask(taskForm:any):Observable<any>{
    let task = {
      name:taskForm.Name,
      description: taskForm.Description,
      coinId:taskForm.CoinId,
      cronExpression:this.ToCronExpr(taskForm.Frequency,taskForm.ExecutionMoment) ,
    }
    console.log(task)
    return this.http.post(this.url+'tasks',task);
  }

  putTask(taskForm:any):Observable<any>{
    let task = {
      id:taskForm.Id,
      name:taskForm.Name,
      description: taskForm.Description,
      coinId:taskForm.CoinId,
      cronExpression:this.ToCronExpr(taskForm.Frequency,taskForm.ExecutionMoment) ,
    }
    return this.http.put(this.url+'tasks',task);
  }

  deleteTask(id:number):Observable<any>{
    return this.http.delete(this.url+'tasks/'+id)
  }
  getCoinTaskById(id:number):Observable<any>{
    return this.http.get(this.url+'tasks/'+id)
  }
  getUserCoinTasks():Observable<any>{
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
