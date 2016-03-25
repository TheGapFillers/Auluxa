import { RouteConfig, ROUTER_DIRECTIVES } from 'angular2/router';
import { Component, OnInit } from 'angular2/core';
import { Router } from 'angular2/router';

@Component({
  selector: 'bill-app',
  templateUrl: './app/user-settings/bill/bill-app/bill-app.component.html',
  directives: [ROUTER_DIRECTIVES]
})

export class BillAppComponent { }
