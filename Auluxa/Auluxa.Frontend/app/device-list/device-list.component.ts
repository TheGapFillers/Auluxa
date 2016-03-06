import { RouteConfig, ROUTER_DIRECTIVES } from 'angular2/router';
import { Component, OnInit } from 'angular2/core';
import { Router } from 'angular2/router';
import { AllComponent } from './all/all.component';
import { AudioVideoComponent } from './audio-video/audio-video.component';
import { ClimateComponent } from './climate/climate.component';
import { LigthingComponent } from './ligthing/ligthing.component';
import { PanelsComponent } from './panels/panels.component';
import { RecentlyAddedComponent } from './recently-added/recently-added.component';
import { ShadesComponent } from './shades/shades.component';
import { ThermosComponent } from './thermos/thermos.component';
import { ThirdPartyComponent } from './third-party/third-party.component';

@Component({
  selector: 'device-list',
  template: `
      <router-outlet></router-outlet>
    `,
  directives: [ROUTER_DIRECTIVES]
})
@RouteConfig([
  { path: '/all', name: 'All', component: AllComponent, useAsDefault: true },
  { path: '/audio-video', name: 'AudioVideo', component: AudioVideoComponent },
  { path: '/climate', name: 'Climate', component: ClimateComponent },
  { path: '/ligthing', name: 'Ligthing', component: LigthingComponent },
  { path: '/panels', name: 'Panels', component: PanelsComponent },
  { path: '/recently-added', name: 'RecentlyAdded', component: RecentlyAddedComponent },
  { path: '/shades', name: 'Shades', component: ShadesComponent },
  { path: '/thermos', name: 'Thermos', component: ThermosComponent },
  { path: '/third-party', name: 'ThirdParty', component: ThirdPartyComponent }
])

export class DeviceListComponent { }
