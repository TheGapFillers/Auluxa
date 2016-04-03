import { RouteParams, ROUTER_DIRECTIVES } from "angular2/router";
import { Component } from "angular2/core";
import { Router } from "angular2/router";

@Component({
    selector: "bill-invoice-detail",
    templateUrl: "./app/pages/store/bill/bill-invoice-detail/bill-invoice-detail.component.html",
    directives: [ROUTER_DIRECTIVES]
})

export class BillInvoiceDetailComponent {
    public type: string;
    public id: string;

    constructor(
        private _router: Router,
        private _routeParams: RouteParams) { }

    public ngOnInit(): any {
        this.type = this._routeParams.get("type");
        this.id = this._routeParams.get("id");
    }
}
