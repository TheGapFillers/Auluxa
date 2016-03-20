import { Injectable } from 'angular2/core';
import { Http, Response } from 'angular2/http';
import { Observable, Subject } from 'rxjs/Rx';

import { CONFIG } from '../../app/shared/config/config';
import { KraniumModel } from './kranium.model';

let kraniumUrl = CONFIG.baseUrls.kranium;

@Injectable()
export class KraniumService {
  constructor(
    private _http: Http
  ) { }

  getKranium(): KraniumModel {
    return {
      "name": "One Kranium",
      "version": 0.1,
      "macAddress": null,
      "ipAddress": "127.0.0.1",
      "zigBeePanId": null,
      "zigBeeChannel": null,
      "zigBeeMacAddress": null
    };
  }
}