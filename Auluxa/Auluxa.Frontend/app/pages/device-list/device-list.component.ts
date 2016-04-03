import { RouteConfig, ROUTER_DIRECTIVES } from "angular2/router";
import { Component } from "angular2/core";

import { AllComponent } from "./all/all.component";
import { AddThirdPartyComponent } from "./add-third-party/add-third-party.component";
import { AddAuluxaComponent } from "./add-auluxa/add-auluxa.component";
import { AudioVideoComponent } from "./audio-video/audio-video.component";
import { ClimateComponent } from "./climate/climate.component";
import { LightingComponent } from "./lighting/lighting.component";
import { PanelsComponent } from "./panels/panels.component";
import { RecentlyAddedComponent } from "./recently-added/recently-added.component";
import { ShadesComponent } from "./shades/shades.component";
import { ThermosComponent } from "./thermos/thermos.component";
import { ThirdPartyComponent } from "./third-party/third-party.component";

@Component({
    selector: "device-list",
    template: `
      <router-outlet></router-outlet>
    `,
    directives: [ROUTER_DIRECTIVES]
})
@RouteConfig([
    { path: "/all", name: "All", component: AllComponent, useAsDefault: true },
    { path: "/add-third-party", name: "AddThirdParty", component: AddThirdPartyComponent },
    { path: "/add-auluxa", name: "AddAuluxa", component: AddAuluxaComponent },
    { path: "/audio-video", name: "AudioVideo", component: AudioVideoComponent },
    { path: "/climate", name: "Climate", component: ClimateComponent },
    { path: "/lighting", name: "Lighting", component: LightingComponent },
    { path: "/panels", name: "Panels", component: PanelsComponent },
    { path: "/recently-added", name: "RecentlyAdded", component: RecentlyAddedComponent },
    { path: "/shades", name: "Shades", component: ShadesComponent },
    { path: "/thermos", name: "Thermos", component: ThermosComponent },
    { path: "/third-party", name: "ThirdParty", component: ThirdPartyComponent }
])

export class DeviceListComponent { }
