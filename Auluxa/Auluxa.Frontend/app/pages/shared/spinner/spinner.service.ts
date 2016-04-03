// import { Injectable } from 'angular2/core';
// import { Observable, Subject } from 'rxjs/Rx';

// export interface ISpinnerState {
//   show: boolean
// }

// @Injectable()
// export class SpinnerService {
//   private _spinnerSubject = new Subject();
//   private _counter = 0;
  
//   spinnerState = <Observable<ISpinnerState>>this._spinnerSubject;

//   show() {
//     this._counter++;
//     this._spinnerSubject.next(<ISpinnerState>{ show: true });
//   }

//   hide() {
//     this._counter--;
//     // Only hides the spinner if the hide method was called has many times as the show.
//     if (this._counter === 0) {
//       this._spinnerSubject.next(<ISpinnerState>{ show: false });
//     }
//   }
// }