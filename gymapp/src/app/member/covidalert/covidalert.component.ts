import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-covidalert',
  templateUrl: './covidalert.component.html',
  styleUrls: ['./covidalert.component.scss'],
})
export class CovidalertComponent implements OnInit {

  covidCollapsed = false;
  hideMessage = false;
  @Input() currentMsg: any;

  constructor() { }

  ngOnInit() {
    this.checkDisplayStatus();
  }

  onCollapse() {

      this.covidCollapsed = !this.covidCollapsed;

  }

  checkDisplayStatus() {
    const status = localStorage.getItem('msgDisplayStatus');

    if (status === 'true') {
      this.covidCollapsed = true;
      this.hideMessage = false;
    } else {
      this.covidCollapsed = false;
      this.hideMessage = true;
    }
  }

  onCheckStatus() {


    console.log(this.hideMessage.toString());
    localStorage.setItem('msgDisplayStatus', this.hideMessage.toString());

  }


}
