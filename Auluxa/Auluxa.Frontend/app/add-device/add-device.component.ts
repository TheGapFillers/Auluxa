import { Component } from 'angular2/core';
import { RouteConfig, ROUTER_DIRECTIVES } from 'angular2/router';
import { AuluxaComponent } from './auluxa/auluxa.component';
import { ThirdPartyComponent } from './third-party/third-party.component';

@Component({
  selector: 'add-device',
  template: `
      <router-outlet></router-outlet>
    `,
  directives: [ROUTER_DIRECTIVES]
})
@RouteConfig([
  { path: '/auluxa', name: 'Auluxa', component: AuluxaComponent, useAsDefault: true },
  { path: '/thirdparty', name: 'ThirdParty', component: ThirdPartyComponent }
])

export class AddDeviceComponent { }
