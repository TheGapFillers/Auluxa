import { RouteConfig, ROUTER_DIRECTIVES } from 'angular2/router';
import { Component, OnInit } from 'angular2/core';
import { Router } from 'angular2/router';

import { UserComponent } from './user/user.component';
import { BillComponent } from './bill/bill.component';
import { BillAppComponent } from './bill/bill-app/bill-app.component';
import { BillProductComponent } from './bill/bill-product/bill-product.component';
import { BillStorageComponent } from './bill/bill-storage/bill-storage.component';
import { BillInvoiceDetailComponent } from './bill/bill-invoice-detail/bill-invoice-detail.component';
import { StoreComponent } from './store/store.component';

@Component({
  selector: 'user-settings',
  template: `
      <router-outlet></router-outlet>
    `,
  directives: [ROUTER_DIRECTIVES]
})
@RouteConfig([
  { path: '/user', name: 'User', component: UserComponent, useAsDefault: true },
  { path: '/bill', name: 'Bill', component: BillComponent },
  { path: '/bill/app', name: 'Bill_App', component: BillAppComponent },
  { path: '/bill/product', name: 'Bill_Product', component: BillProductComponent },
  { path: '/bill/storage', name: 'Bill_Storage', component: BillStorageComponent },
  { path: '/bill/:type/invoice/:id', name: 'Bill_InvoiceDetail', component: BillInvoiceDetailComponent },
  { path: '/store', name: 'Store', component: StoreComponent }
])

export class UserSettingsComponent { }
