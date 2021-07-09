import { Component, OnInit, ViewChild } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { IonSlides, ModalController } from '@ionic/angular';
import { CheckinComponent } from '../checkin/checkin.component';
import { ProfileComponent } from '../profile/profile.component';
import { Router } from '@angular/router';
import { SecurityService } from 'src/app/api/security.service';
import { environment } from 'src/environments/environment';
import { SharedService } from 'src/app/api/shared.service';
import { PackageDetailPageRoutingModule } from 'src/app/pkgs/package-detail/package-detail-routing.module';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.page.html',
  styleUrls: ['./schedule.page.scss'],
})
export class SchedulePage implements OnInit {

  currentDay = 0;
  scheduleCollapsed = false;
  currentMenu: any;
  currentUser: any;
  basePhotosUrl = environment.profilesPhotosRepoUrl + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/';
  covidMsg: any;
  scheduleSummary: any;
  scheduleWeekDays: any;
  todayBooked: any;

  sliderConfig = {
    initialSlide: 0,
    centeredSlides: true,
    slidesPerView: this.checkScreen() // 1.2
  };

  memberSchedule: any;
  @ViewChild('slides', {static: false}) slides: IonSlides;

  constructor(private gymService: GymService, private securityService: SecurityService,
              private modalCtrl: ModalController, private router: Router,
              private sharedService: SharedService) {
   }

  ngOnInit() {
    this.memberSchedule = this.gymService.getMemberSchedule(123);

    this.currentUser = this.securityService.getCurrentLoggedUser();
    this.currentMenu = this.securityService.getMenuByCurrentUserRole();

    this.sharedService.onUpdateData.subscribe(value => {
      if (value === 'profile') {
        this.currentUser = this.securityService.getCurrentLoggedUser();
      }
    });

  }

  ionViewDidEnter() {
    this.loadCovidMessage();
    this.loadScheduleSummaryData();
    this.loadScheduleWeekDays();
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
        return 3.3;
    }

  }

  getDisplayMonth(date: any) {

    const itmDate = new Date(date);
    const month = itmDate.getMonth();

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
    console.log('determinar dia: ', date);

    const weekDays = ['Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado', 'Domingo'];

    result = result + weekDays[date.getDay()];
    result = result + ' ' + date.getDate();
    result = result + ' ' + this.getDisplayMonth(date.getMonth());
    // result = result + date.getYear();

    return result;
  }

  async checkIn() {
    const modal = await this.modalCtrl.create({
      component: CheckinComponent,
      componentProps: { date: this.todayBooked.date, bookedHour: this.todayBooked.bookedHour }
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

  loadCovidMessage() {
    this.gymService.getCovidMessage().subscribe(response => {
      this.covidMsg = response;
    });
  }

  loadScheduleSummaryData() {

    this.gymService.getMemberScheduleSummaryByUserId(this.currentUser.id).subscribe(response => {
      this.scheduleSummary = response;
      this.checkTodayBook();
    });
  }

  checkTodayBook() {

    if (!this.scheduleSummary || this.scheduleSummary.length === 0) {
      return;
    }

    const bookedDate = new Date(this.scheduleSummary[0].date);
    const today = new Date();

    if (bookedDate.getMonth === today.getMonth && bookedDate.getDate() === today.getDate()) {
      this.todayBooked = this.scheduleSummary[0];
    }

  }

  loadScheduleWeekDays() {
    this.gymService.getScheduleWeekly(this.currentUser.id).subscribe(response => {
      this.scheduleWeekDays = response;
    });
  }

  updateReservation(date: any, status: any, range: any, schedule: any, daily: any) {

    const selDate = new Date(date);

    const month = '00' + (selDate.getMonth() + 1);
    const day = '00' + selDate.getDate();

    const formattedDate = selDate.getFullYear() + month.substring(month.length - 2) + day.substring(day.length - 2);
    const formattedHour = range.substring(0, 5).replace(':', '');

    if (!status) {
      this.gymService.bookMemberDate(this.currentUser.id, formattedDate, formattedHour).subscribe(response => {
        this.gymService.getScheduleByDayAndUser(this.currentUser.id, date).subscribe(resp => {
          daily.selectableHours = resp.selectableHours;
          this.loadScheduleSummaryData();
        });
      }, error => {
        schedule.booked = status;
      });
    } else {
      this.gymService.deleteMemberDate(this.currentUser.id, formattedDate, formattedHour).subscribe(() => {
        this.gymService.getScheduleByDayAndUser(this.currentUser.id, date).subscribe(resp => {
          daily.selectableHours = resp.selectableHours;
          this.loadScheduleSummaryData();
        });

      });
    }

  }

  logOut() {
    this.securityService.logOut();
    this.router.navigateByUrl('/home');
  }
}
