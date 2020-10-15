import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-covidalert',
  templateUrl: './covidalert.component.html',
  styleUrls: ['./covidalert.component.scss'],
})
export class CovidalertComponent implements OnInit {

  covidCollapsed = false;

  constructor() { }

  ngOnInit() {}

  onCollapse() {

      this.covidCollapsed = !this.covidCollapsed;

  }


}
