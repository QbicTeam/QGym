import { Component, OnInit, ViewChild } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { IonSlides } from '@ionic/angular';
import { SecurityService } from 'src/app/api/security.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-frontdesk',
  templateUrl: './frontdesk.page.html',
  styleUrls: ['./frontdesk.page.scss'],
})
export class FrontdeskPage implements OnInit {

  basePhotosUrl = environment.profilesPhotosRepoUrl + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/';


  currentView = 'block';
  data: any;
  dataSearched: any;
  currentDay = 0;
  currentMenu: any;
  currentUser: any;

  sliderConfig = {
    initialSlide: 0,
    centeredSlides: true,
    slidesPerView: this.checkScreen() // 1.2
  };

  generalSchedule: any;
  @ViewChild('slides', {static: false}) slides: IonSlides;

  constructor(private gymService: GymService, private securityService: SecurityService, private router: Router) { }


  ngOnInit() {
    this.currentUser = this.securityService.getCurrentLoggedUser();
    this.currentMenu = this.securityService.getMenuByCurrentUserRole();

    this.generalSchedule = this.gymService.getGeneralSchedule();
  }

  goto(slideNumber) {
    this.slides.slideTo(slideNumber, 2000);
  }

  checkScreen() {

    const innerWidth = window.innerWidth;

    switch (true) {
      case 340 <= innerWidth && innerWidth <= 400:
        return 1.2;
      case 401 <= innerWidth && innerWidth <= 700:
        return 1.3;
      case 701 <= innerWidth && innerWidth <= 900:
        return 2.3;
      case 901 <= innerWidth:
        return 2.3;
    }
  }

  getDisplayMonth(month: number) {

    const months = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];

    return months[month];
  }

  getIndex(event) {
    this.slides.getActiveIndex().then(idx => {
      this.currentDay = idx;
    });
  }

  getColor(percentage) {

    let result = '';

    if (percentage > 1 && percentage < 26) {
      result = 'warning';
    } else if (percentage > 25) {
      result = 'success';
    } else if (percentage === 0) {
      result = 'danger';
    }

    return result;

  }

  getAvailability(scheduleDate, hour) {

    let result = true;

    const scheduleHour = +hour.substring(0, 2);
    const currentDate = new Date();
    const currentMinutes = currentDate.getMinutes();
    const currentHour = currentDate.getHours();

    const currentDay = currentDate.getDate();
    const scheduleDay = scheduleDate.getDate();

    if (currentDay === scheduleDay) {

      if (scheduleHour < currentHour) {
        result = false;
      } else if (scheduleHour === currentHour && currentMinutes > 15) {
        result = false;
      }

    }

    return result;

  }

  onMyProfile() {

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
