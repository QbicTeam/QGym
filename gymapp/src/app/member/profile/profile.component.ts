import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { GymService } from 'src/app/api/gym.service';
import { SecurityService } from 'src/app/api/security.service';
import { SharedService } from 'src/app/api/shared.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {

  currentUser: any;

  constructor(private securityService: SecurityService, private gymService: GymService,
              private modalCtrl: ModalController, private sharedService: SharedService) { }

  ngOnInit() {

    this.loadCurrentUserData();

  }

  loadCurrentUserData() {

    const userData = this.securityService.getCurrentLoggedUser();

    this.gymService.getMemberDetailsById(userData.id).subscribe(response => {
      this.currentUser = response;
      console.log('loaded user: ', this.currentUser);
    });

  }

  onUpdateMemberData() {
    this.gymService.updateMemberData(this.currentUser).subscribe(() => {
      this.securityService.setNameToCurrentLoggedUser(this.currentUser.fullName);
      this.sharedService.setOnUpdateData('profile');
    });
  }

  onExit() {
    this.modalCtrl.dismiss();
  }

}
