import { Component } from "angular2/core";

@Component({
    selector: "header",
    templateUrl: "./app/pages/shared/header/header.component.html"
})

export class HeaderComponent {
    public firstName: string = "Ambroise";
    public lastName: string = "Coussin";
    public accountType: string = "Admin";
    public accountStatus: string = "Free";
    public couponCode: string = "1231233";
    public version: string = "1.7";

    constructor(
    ) { }
}
