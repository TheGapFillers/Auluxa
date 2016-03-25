import { RouteConfig, RouteParams, ROUTER_DIRECTIVES } from 'angular2/router';
import { Component, OnInit } from 'angular2/core';
import { Router } from 'angular2/router';

@Component({
  selector: 'bill-invoice-detail',
  templateUrl: './app/user-settings/bill/bill-invoice-detail/bill-invoice-detail.component.html',
  directives: [ROUTER_DIRECTIVES]
})

export class BillInvoiceDetailComponent {
  public type: string;
  public parentRoute: string;

  constructor(
    private _router: Router,
    private _routeParams: RouteParams) { }

  ngOnInit() {
    this.type = this._routeParams.get('type');
    this.parentRoute = 'Bill_' + this.type;
    let id = this._routeParams.get('id');
  }
}
