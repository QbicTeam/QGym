import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GymService } from 'src/app/api/gym.service';
import { SecurityService } from 'src/app/api/security.service';
import { SharedService } from 'src/app/api/shared.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.page.html',
  styleUrls: ['./configuration.page.scss'],
})
export class ConfigurationPage implements OnInit {

  basePhotosUrl = environment.profilesPhotosRepoUrl + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/';


  currentMenu: any;
  currentUser: any;

  currentView = 'block';
  data: any;
  dataSearched: any;

  membersData: any;
  membersDataSearched: any;

  capacityData: any;
  scheduleData: any;

  constructor(private gymService: GymService, private sharedService: SharedService,
              private securityService: SecurityService, private router: Router) { }

  ngOnInit() {

    this.initSubscriptions();


    this.currentUser = this.securityService.getCurrentLoggedUser();
    this.currentMenu = this.securityService.getMenuByCurrentUserRole();

    console.log(this.currentUser);

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

  onMyProfile() {

  }

  onOptionSelected() {
    console.log(this.currentView);
    if (this.currentView === 'block') {
      this.loadActiveMemberList();
    } else if (this.currentView === 'ocupation') {
      this.loadCapacityData();
    } else if (this.currentView === 'schedule') {
      this.loadScheduleData();
    // } else if (this.currentView === 'users') {
    //   this.loadUsersData();
    } else if (this.currentView === 'packages') {
      this.loadMembershipsData();
    }
  }

  ionViewDidEnter() {
    this.loadActiveMemberList();
  }

  loadActiveMemberList() {

    this.gymService.getActiveMembersList().subscribe(response => {

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
    // this.gymService.getMembersList().subscribe(response => {
    //   console.log('Users list members retrieved...');
    //   this.membersData = response;
    //   this.membersDataSearched = this.data;
    // });
  }

  loadMembershipsData() {
    console.log('loading memberships data...');
  }


  onSelectedOption(option) {

    console.log(option);

    if (option === 'packages') {
      this.router.navigate(['/packages']);
    }
    else if (option === 'schedule') {
      this.router.navigate(['/schedule']);
    }
    else if (option === 'frontdesk') {
      this.router.navigate(['/frontdesk']);
    }
    else if (option === 'admin') {
      this.router.navigate(['/configuration']);
    }
    else if (option === 'sales') {
      this.router.navigate(['/sales-report']);
    }
  }

}
