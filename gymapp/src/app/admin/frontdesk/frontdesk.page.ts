import { Component, OnInit, ViewChild } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { IonSlides } from '@ionic/angular';

@Component({
  selector: 'app-frontdesk',
  templateUrl: './frontdesk.page.html',
  styleUrls: ['./frontdesk.page.scss'],
})
export class FrontdeskPage implements OnInit {

  currentView = 'block';
  data: any[] = [];
  dataSearched: any[];
  currentDay = 0;

  sliderConfig = {
    initialSlide: 0,
    centeredSlides: true,
    slidesPerView: this.checkScreen() // 1.2
  };

  generalSchedule: any;
  @ViewChild('slides', {static: false}) slides: IonSlides;

  constructor(private gymService: GymService) { }


  ngOnInit() {
    this.data = this.gymService.getMembersList();
    this.dataSearched = this.data;
    this.generalSchedule = this.gymService.getGeneralSchedule();
  }

  searchMember(event) {

    const val = event.target.value;

    this.dataSearched = this.data;
    if (val && val.trim() !== '' ) {
      this.dataSearched = this.dataSearched.filter((item: any) => {
        return (item.searchText.toLowerCase().indexOf(val.toLowerCase()) > -1);
      });
    }

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

}
