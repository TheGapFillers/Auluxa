import { Component, provide } from 'angular2/core';
import { HTTP_PROVIDERS } from 'angular2/http';
import 'rxjs/Rx'; // load the full rxjs

import { RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS } from 'angular2/router';
import { AddDeviceComponent } from './add-device/add-device.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { KraniumComponent, KraniumService } from './kranium/kranium';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { CameraSettingsComponent } from './camera-settings/camera-settings.component';
import { EnergySettingsComponent } from './energy-settings/energy-settings.component';
import { ImageLibraryComponent } from './image-library/image-library.component';
import { IRLibraryComponent } from './ir-library/ir-library.component';
import { StorageComponent } from './storage/storage.component';
import { TriggersComponent } from './triggers/triggers.component';

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
  { path: '/user-settings/...', name: 'UserSettings', component: UserSettingsComponent },
  { path: '/camera-settings', name: 'CameraSettings', component: CameraSettingsComponent },
  { path: '/energy-settings', name: 'EnergySettings', component: EnergySettingsComponent },
  { path: '/image-library', name: 'ImageLibrary', component: ImageLibraryComponent },
  { path: '/ir-library', name: 'IRLibrary', component: IRLibraryComponent },
  { path: '/storage', name: 'Storage', component: StorageComponent },
  { path: '/triggers', name: 'Triggers', component: TriggersComponent },
])

export class AppComponent {
  constructor(
    private _kraniumService: KraniumService
    
  ){}
}