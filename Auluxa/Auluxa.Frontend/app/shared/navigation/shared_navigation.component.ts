import { Component, OnInit } from 'angular2/core';
import { ROUTER_DIRECTIVES } from 'angular2/router';

@Component({
  selector: 'shared_navigation',
  templateUrl: './app/shared/navigation/shared_navigation.component.html',
  directives: [ROUTER_DIRECTIVES]
})
export class Shared_NavigationComponent { }
