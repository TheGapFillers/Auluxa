import { Injectable } from 'angular2/core';
import { Http, Response, Headers, RequestOptions } from 'angular2/http';
import { Observable, Subject } from 'rxjs/Rx';
import 'rxjs/Rx';

import { CONFIG } from '../../app/shared/config/config';
import { KraniumModel } from './kranium.model';

let kraniumUrl = CONFIG.baseUrls.kranium;

@Injectable()
export class KraniumService {
  constructor(
    private _http: Http
  ) { }

  getKranium(): Observable<KraniumModel> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });

    return this._http.get('http://auluxawebapp-prod.ap-southeast-1.elasticbeanstalk.com/api/kranium', options)
      .map(res => <KraniumModel>res.json().data)
      .do(data => console.log(data));

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