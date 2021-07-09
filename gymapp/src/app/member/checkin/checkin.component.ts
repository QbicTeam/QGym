import { Component, Input, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { GymService } from 'src/app/api/gym.service';
import { SecurityService } from 'src/app/api/security.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-checkin',
  templateUrl: './checkin.component.html',
  styleUrls: ['./checkin.component.scss'],
})
export class CheckinComponent implements OnInit {

  @Input() date: any;
  @Input() bookedHour: any;

  currentUser: any;
  memberDetails: any;
  basePhotosUrl = environment.profilesPhotosRepoUrl + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/';

  constructor(private modalCtrl: ModalController, private securyService: SecurityService, private gymService: GymService) { }

  ngOnInit() {
    this.currentUser = this.securyService.getCurrentLoggedUser();
    this.loadMemberData();
  }

  dismissModal() {
    this.modalCtrl.dismiss();
  }

  getStatusColor() {

    let result = 'danger';

    const currentDate = new Date();
    const formattedDate = currentDate.getFullYear() + '/' + (currentDate.getMonth() + 1) + '/' + currentDate.getDate();
    const todayBooked = new Date(formattedDate + ' ' + this.bookedHour.substring(0, 5));
    const todayEndsBooked = new Date(formattedDate + ' ' + this.bookedHour.substring(8));

    const startdate = new Date(todayBooked);
    startdate.setMinutes(-10);

    if (this.memberDetails && !this.memberDetails.isBlock) {

      if (currentDate >= startdate && currentDate <= todayEndsBooked ) {
        result = 'success';

        if (!this.memberDetails.isVerified) {
          result = 'warning';
        }

      }

    }

    return result;

  }

  loadMemberData() {
    this.gymService.getMemberDetailsById(this.currentUser.id).subscribe(response => {
      this.memberDetails = response;
    });
  }



}
