import { Component, OnInit } from "angular2/core";
import { KraniumService } from "./kranium.service";
import { KraniumModel } from "./kranium.model";

@Component({
    selector: "kranium",
    templateUrl: "./app/pages/kranium/kranium.component.html",
    providers: [
        KraniumService
    ]
})

export class KraniumComponent implements OnInit {
    public kranium: KraniumModel;

    constructor(
        private _kraniumService: KraniumService
    ) { }

    public ngOnInit(): void {
        // this.getKRanium();
    }

    public getKRanium(): any {
        this._kraniumService.getKranium()
            .subscribe(
            (kranium: any) => this.kranium = kranium);
    }
}
