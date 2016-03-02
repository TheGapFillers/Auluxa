import { Component, OnInit } from 'angular2/core';
import { Router } from 'angular2/router';

@Component({
  selector: 'shared_header',
  templateUrl: './app/shared/header/shared_header.component.html'
})
export class Shared_HeaderComponent {
  public firstName = 'Dmints';
  public lastName = 'Bambi';
  public accountType = 'Admin';
  public accountStatus = 'Free';
  public couponCode = '1231233';
  public version = '1.7';

  constructor(
  ) { }
}
