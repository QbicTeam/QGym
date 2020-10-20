import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { SharedService } from 'src/app/api/shared.service';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.page.html',
  styleUrls: ['./configuration.page.scss'],
})
export class ConfigurationPage implements OnInit {

  currentView = 'block';
  data: any;
  dataSearched: any;

  capacityData: any;
  scheduleData: any;

  constructor(private gymService: GymService, private sharedService: SharedService) { }

  ngOnInit() {
    this.initSubscriptions();
  }

  initSubscriptions() {

    // this.sharedService.onConfigurationOptionSelected.subscribe(valueSelected => {
    //   this.loadActiveMemberList();
    // });

    this.sharedService.onConfigurationOptionSelected.subscribe(valueSelected => {
      if (valueSelected === 'ocupation') {
        this.loadCapacityData();
      }
    });

  }

  onOptionSelected() {
    console.log(this.currentView);
    if (this.currentView === 'block') {
      this.loadActiveMemberList();
    } else if (this.currentView === 'ocupation') {
      this.loadCapacityData();
    } else if (this.currentView === 'schedule') {
      this.loadScheduleData();
    } else if (this.currentView === 'users') {
      this.loadUsersData();
    } else if (this.currentView === 'packages') {
      this.loadMembershipsData();
    }
  }

  ionViewDidEnter() {
    this.loadActiveMemberList();
  }

  loadActiveMemberList() {

    this.gymService.getMembersList().subscribe(response => {

      this.data = response;
      this.dataSearched = this.data;

    });
  }

  loadCapacityData() {
    console.log('loading capacity data...');
    this.gymService.getCapacityData().subscribe(response => {
      this.capacityData = response;
      console.log('Capacity response: ', response);
    });
  }

  loadScheduleData() {
    this.gymService.getScheduleConfiguration().subscribe(response => {
      this.scheduleData = response;
    });
  }

  loadUsersData() {
    console.log('loading users data');
  }

  loadMembershipsData() {
    console.log('loading memberships data...');
  }


}
