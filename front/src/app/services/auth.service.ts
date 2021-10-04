import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {tap } from 'rxjs/operators'
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { LoginModel } from '../models/loginModel';
import { Token } from '../models/token';


export const ACCESS_TOKEN_KEY = 'access token'

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  url = "https://localhost:5001/api/"
  constructor(private http:HttpClient,private jwtHelper:JwtHelperService,private router:Router) { }

  signup(loginModel:LoginModel):Observable<Token>{
    return this.http.post<Token>(this.url+'Auth/register',loginModel).pipe(tap(token=>{
      localStorage.setItem(ACCESS_TOKEN_KEY,token.access_token)
      this.router.navigate(['coffee'])
    }))
  }

  login(loginModel:LoginModel):Observable<Token>{
    return this.http.post<Token>(this.url + 'Auth/login',loginModel).pipe(tap(token=>{
          localStorage.setItem(ACCESS_TOKEN_KEY,token.access_token)
          this.router.navigate(['coffee'])
        }))
  }

  isAuthenticated():boolean{
    let token = localStorage.getItem(ACCESS_TOKEN_KEY);
    return token!=null && !this.jwtHelper.isTokenExpired(token)
  }

  logout():void{
    localStorage.removeItem(ACCESS_TOKEN_KEY)
    this.router.navigate([''])
  }
  getUserInfo(){
    return this.http.get(this.url+'users/account')
  }
  getAllUsers():Observable<any[]>{
    return this.http.get<any[]>(this.url + 'users')
  }

}
