import { Component, OnInit } from 'angular2/core';
import { Router } from 'angular2/router';
import { KraniumService } from './kranium.service';
import { KraniumModel } from './kranium.model';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'kranium',
  templateUrl: './app/kranium/kranium.component.html',
  providers: [
    KraniumService
  ]
})
export class KraniumComponent implements OnInit {
  kranium: KraniumModel;

  constructor(
    private _kraniumService: KraniumService
  ) { }

  ngOnInit() {
    this.getKRanium();
  }
  
  getKRanium(){
    this._kraniumService.getKranium()
                     .subscribe(
                       kranium => this.kranium = kranium);
  }
}
