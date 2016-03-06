import { RouteConfig, ROUTER_DIRECTIVES } from 'angular2/router';
import { Component, OnInit } from 'angular2/core';
import { Router } from 'angular2/router';
import { UserComponent } from './user/user.component';
import { BillComponent } from './bill/bill.component';

@Component({
  selector: 'user-settings',
  template: `
      <router-outlet></router-outlet>
    `,
  directives: [ROUTER_DIRECTIVES]
})
@RouteConfig([
  { path: '/user', name: 'User', component: UserComponent, useAsDefault: true },
  { path: '/bill', name: 'Bill', component: BillComponent }
])

export class UserSettingsComponent { }
