import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-show-apis',
  templateUrl: './show-apis.component.html',
  styleUrls: ['./show-apis.component.scss']
})
export class ShowApisComponent implements OnInit {

  constructor(private router:Router) { }
  header:string=""
  ngOnInit(): void {
    this.header = this.router.url.substring(1)
  }

}
