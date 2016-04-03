import { RouteConfig, ROUTER_DIRECTIVES } from "angular2/router";
import { Component } from "angular2/core";

import { LicenseComponent } from "./license/license.component";
import { StorageComponent } from "./storage/storage.component";
import { BillComponent } from "./bill/bill.component";
import { BillAppComponent } from "./bill/bill-app/bill-app.component";
import { BillProductComponent } from "./bill/bill-product/bill-product.component";
import { BillStorageComponent } from "./bill/bill-storage/bill-storage.component";
import { BillInvoiceDetailComponent } from "./bill/bill-invoice-detail/bill-invoice-detail.component";

@Component({
    selector: "user-settings",
    template: `
      <router-outlet></router-outlet>
    `,
    directives: [ROUTER_DIRECTIVES]
})

@RouteConfig([
    { path: "/license", name: "License", component: LicenseComponent, useAsDefault: true },
    { path: "/storage", name: "Storage", component: StorageComponent },
    { path: "/bill", name: "Bill", component: BillComponent },
    { path: "/bill/app", name: "Bill_App", component: BillAppComponent },
    { path: "/bill/product", name: "Bill_Product", component: BillProductComponent },
    { path: "/bill/storage", name: "Bill_Storage", component: BillStorageComponent },
    { path: "/bill/:type/invoice/:id", name: "Bill_InvoiceDetail", component: BillInvoiceDetailComponent }
])

export class StoreComponent { }
