import { Component } from "angular2/core";
import { HTTP_PROVIDERS } from "angular2/http";
import "rxjs/Rx"; // load the full rxjs

import { RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS } from "angular2/router";

import * as pages from "./pages/pages-exports";

@Component({
    selector: "auluxa-app",
    templateUrl: "app/app.component.html",
    directives: [
        ROUTER_DIRECTIVES,
        pages.HeaderComponent,
        pages.NavigationComponent,
        pages.FooterComponent],
    providers: [
        ROUTER_PROVIDERS,
        HTTP_PROVIDERS,
        // KraniumService
    ]
})

@RouteConfig([
    { path: "/dashboard", name: "Dashboard", component: pages.DashboardComponent, useAsDefault: true },
    { path: "/device-list/...", name: "DeviceList", component: pages.DeviceListComponent },
    { path: "/kranium", name: "Kranium", component: pages.KraniumComponent },
    { path: "/storage", name: "Storage", component: pages.StorageComponent },
    { path: "/store/...", name: "Store", component: pages.StoreComponent },
    { path: "/triggers", name: "Triggers", component: pages.TriggersComponent },
    { path: "/users", name: "Users", component: pages.UsersComponent },
])

export class AppComponent {
    constructor() { }
}
