import { Component, provide } from 'angular2/core';
import { HTTP_PROVIDERS } from 'angular2/http';
import 'rxjs/Rx'; // load the full rxjs

import { RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS } from 'angular2/router';
import { AddDeviceComponent } from './add-device/add-device.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { KraniumComponent, KraniumService } from './kranium/kranium';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { Shared_HeaderComponent } from './shared/header/shared_header.component';
import { Shared_NavigationComponent } from './shared/navigation/shared_navigation.component';
import { Shared_FooterComponent } from './shared/footer/shared_footer.component';


@Component({
  selector: 'auluxa-app',
  templateUrl: 'app/app.component.html',
  directives: [
    ROUTER_DIRECTIVES,
    Shared_HeaderComponent,
    Shared_NavigationComponent,
    Shared_NavigationComponent],
  providers: [
    ROUTER_PROVIDERS,
    HTTP_PROVIDERS,
    KraniumService
  ]
})
@RouteConfig([
  { path: '/add-device/...', name: 'AddDevice', component: AddDeviceComponent },
  { path: '/dashboard', name: 'Dashboard', component: DashboardComponent, useAsDefault: true },
  { path: '/device-list/...', name: 'DeviceList', component: DeviceListComponent },
  { path: '/kranium', name: 'Kranium', component: KraniumComponent },
  { path: '/user-settings/...', name: 'UserSettings', component: UserSettingsComponent }
])

export class AppComponent {
  constructor(
    private _kraniumService: KraniumService
    
  ){}
}