import { Injectable } from "angular2/core";
import { Http, Headers, RequestOptions } from "angular2/http";
import { Observable } from "rxjs/Rx";
import "rxjs/Rx";

import { CONFIG } from "../../config/config";
import { KraniumModel } from "./kranium.model";

let kraniumUrl = CONFIG.baseUrls.kranium;

@Injectable()
export class KraniumService {
    constructor(
        private _http: Http
    ) { }

    public getKranium(): Observable<KraniumModel> {
        let headers = new Headers({ "Content-Type": "application/json" });
        let options = new RequestOptions({ headers: headers });

        return this._http.get(kraniumUrl, options)
            .map((res: any) => <KraniumModel>res.json().data)
            .do((data: any) => console.log(data));

        // return {
        //   "name": "One Kranium",
        //   "version": 0.1,
        //   "macAddress": null,
        //   "ipAddress": "127.0.0.1",
        //   "zigBeePanId": null,
        //   "zigBeeChannel": null,
        //   "zigBeeMacAddress": null
        // };
    }
}