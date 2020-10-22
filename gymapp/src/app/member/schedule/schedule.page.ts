import { Component, OnInit, ViewChild } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { IonSlides, ModalController } from '@ionic/angular';
import { CheckinComponent } from '../checkin/checkin.component';
import { ProfileComponent } from '../profile/profile.component';
import { Router } from '@angular/router';
import { SecurityService } from 'src/app/api/security.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.page.html',
  styleUrls: ['./schedule.page.scss'],
})
export class SchedulePage implements OnInit {

  currentDay = 0;
  scheduleCollapsed = false;
  currentMenu: any;

  sliderConfig = {
    initialSlide: 0,
    centeredSlides: true,
    slidesPerView: this.checkScreen() // 1.2
  };

  memberSchedule: any;
  @ViewChild('slides', {static: false}) slides: IonSlides;

  constructor(private gymService: GymService, private securityService: SecurityService,
              private modalCtrl: ModalController, private router: Router) {
   }

  ngOnInit() {
    this.memberSchedule = this.gymService.getMemberSchedule(123);
    this.currentMenu = this.securityService.getMenuByCurrentUserRole();
    console.log(this.currentMenu);
  }

  checkScreen() {

    const innerWidth = window.innerWidth;

    switch (true) {
      case 340 <= innerWidth && innerWidth <= 400:
        return 1.2;
      case 401 <= innerWidth && innerWidth <= 700:
        return 2.3;
      case 701 <= innerWidth && innerWidth <= 900:
        return 3.3;
      case 901 <= innerWidth:
        return 4.3;
    }

  }

  getDisplayMonth(month: number) {

    const months = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];

    return months[month];
  }

  goto(slideNumber) {
    this.slides.slideTo(slideNumber, 2000);
  }

  showReservation() {
    this.checkIn();
  }

  getIndex(event) {
    this.slides.getActiveIndex().then(idx => {
      this.currentDay = idx;
    });
  }

  getFormmattedDate(date) {
    let result = '';

    const weekDays = ['Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado', 'Domingo'];

    result = result + weekDays[date.getDay()];
    result = result + ' ' + date.getDate();
    result = result + ' ' + this.getDisplayMonth(date.getMonth());
    // result = result + date.getYear();

    return result;
  }

  async checkIn() {
    const modal = await this.modalCtrl.create({
      component: CheckinComponent
    });

    await modal.present();
  }

  async openEditProfile() {
    const modal = await this.modalCtrl.create({
      component: ProfileComponent
    });

    await modal.present();
  }


  onMyProfile() {
    this.openEditProfile();
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
