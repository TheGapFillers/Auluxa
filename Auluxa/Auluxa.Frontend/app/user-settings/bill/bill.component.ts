//import { RouteConfig, ROUTER_DIRECTIVES } from 'angular2/router';
import { Component, OnInit } from 'angular2/core';
import { Router } from 'angular2/router';

// import { AppComponent } from './app/app.component';
// import { ProductComponent } from './product/product.component';
// import { StorageComponent } from './storage/storage.component';

@Component({
  selector: 'bill',
  templateUrl: './app/user-settings/bill/bill.component.html',
  //directives: [ROUTER_DIRECTIVES]
})
// @RouteConfig([
//   //{ path: '/bill', name: 'Bill', component: BillComponent, useAsDefault: true },
//   { path: '/app', name: 'App', component: AppComponent },
//   { path: '/product', name: 'Product', component: ProductComponent },
//   { path: '/storage', name: 'Storage', component: StorageComponent }
// ])

export class BillComponent { }
