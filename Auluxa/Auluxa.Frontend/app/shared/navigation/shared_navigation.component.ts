import { Component, OnInit } from 'angular2/core';
import { ROUTER_DIRECTIVES, Router } from 'angular2/router';
import {NgClass} from 'angular2/common';

@Component({
  selector: 'shared_navigation',
  templateUrl: './app/shared/navigation/shared_navigation.component.html',
  directives: [ROUTER_DIRECTIVES, NgClass]
})
export class Shared_NavigationComponent implements OnInit {
  currentParentRouteName = '';

  constructor(private _router: Router) { }

 // Sets the current parent route name from the router information.
 ngOnInit() {
    // Subscrive to a change in the current URL
    this._router.subscribe((url) => {
      // Gets the information from the URL
      this._router.recognize(url).then((instruction) => {
        // Sets the currentParentRouteName
        if (instruction.component.urlPath === '') {
          this.currentParentRouteName = 'dashboard'
        }
        this.currentParentRouteName = instruction.component.urlPath;
      });
    });
  }

  getClasses(routeName: string, isColapse: boolean) {
    var classes = isColapse ? 'main-nav--collapsible ' : '';
    classes += this.currentParentRouteName === routeName ? 'main-nav--active' : '';
    return classes;
  }
}
