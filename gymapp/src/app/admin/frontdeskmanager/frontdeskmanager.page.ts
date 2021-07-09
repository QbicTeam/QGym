import { Component, OnInit, ViewChild } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { ModalController } from '@ionic/angular';
import { SelectmemberModalComponent } from '../selectmember-modal/selectmember-modal.component';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-frontdeskmanager',
  templateUrl: './frontdeskmanager.page.html',
  styleUrls: ['./frontdeskmanager.page.scss'],
})
export class FrontdeskmanagerPage implements OnInit {

  currentDay = 0;
  currentHour = 0;
  dailyRangeSchedule: any;
  rangeHours: any;

  data: any;
  dataSearched: any;
  selectedMember: any;
  isMemberPinned = false;
  selectedRange = 0;
  basePhotosUrl = environment.profilesPhotosRepoUrl + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/';


  constructor(private gymService: GymService, private modalCtrl: ModalController, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

    // this.data = this.gymService.getMembersDetailsList();
    // this.dataSearched = this.data;


    this.activatedRoute.paramMap.subscribe(paramMap => {

      if (paramMap.has('date') && paramMap.has('hour')) {

        this.loadScheduleWeekDays(paramMap.get('date'), paramMap.get('hour'));

      }

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


  getDisplayMonth(date: number) {

    const itmDate = new Date(date);
    const month = itmDate.getMonth();

    const months = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];

    return months[month];
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

  onAddNewMember() {
    this.addMemberModal();
  }

  async addMemberModal() {
    const modal = await this.modalCtrl.create({
      component: SelectmemberModalComponent,
      componentProps: { currentDate:  this.dailyRangeSchedule[this.currentDay].date}
    });

    modal.onDidDismiss().then(resulData => {
      this.selectedMember = resulData.data;
      this.isMemberPinned = false;
    });

    await modal.present();
  }

  onPinMember() {
    this.isMemberPinned = !this.isMemberPinned;

    if (!this.isMemberPinned) {
      this.selectedMember = null;
    }

  }

  loadScheduleWeekDays(selectedDate: any, selectedHour: any) {
    this.gymService.getFrontDeskScheduleWeekly().subscribe(response => {

      console.log(response);
      this.dailyRangeSchedule = response;
      this.loadCurrentDayData(selectedDate, selectedHour);

    });
  }

  loadCurrentDayData(day: any, hour: any) {

    this.currentDay = day;
    this.currentHour = hour;
    this.rangeHours = this.dailyRangeSchedule[day].selectableHours;

    console.log('Hours: ', this.rangeHours);

    this.selectedRange = this.dailyRangeSchedule[day].selectableHours[hour];

    console.log(this.selectedRange);

    this.loadBookedMembers(this.dailyRangeSchedule[day].date, this.dailyRangeSchedule[day].selectableHours[hour].range);
  }

  loadBookedMembers(date, hour) {

    this.gymService.getBookedMembers(date, hour).subscribe(response => {
      console.log('Booked members..', response);
      this.data = response;
      this.dataSearched = this.data;
    });
  }

  goto(idx) {
    this.currentDay = idx;
    this.currentHour = 0;
    this.loadCurrentDayData(this.currentDay, this.currentHour);
  }

  onHourChanged() {

    const pos = this.dailyRangeSchedule[this.currentDay].selectableHours.indexOf(this.selectedRange);
    this.currentHour = pos;
    console.log('Changed: ', this.currentHour);
    // this.loadScheduleWeekDays(this.currentDay, this.currentHour);
    this.loadBookedMembers(this.dailyRangeSchedule[this.currentDay].date,
      this.dailyRangeSchedule[this.currentDay].selectableHours[this.currentHour].range);

  }

  onBookMember() {

    const selDate = new Date(this.dailyRangeSchedule[this.currentDay].date);

    const month = '00' + (selDate.getMonth() + 1);
    const day = '00' + selDate.getDate();

    const formattedDate = selDate.getFullYear() + month.substring(month.length - 2) + day.substring(day.length - 2);

    const formmatedHour = this.dailyRangeSchedule[this.currentDay].selectableHours[this.currentHour].range.substring(0, 2)
      + this.dailyRangeSchedule[this.currentDay].selectableHours[this.currentHour].range.substring(3, 5);


    console.log(this.selectedMember);
    this.gymService.bookMemberDate(this.selectedMember.userId,
      formattedDate, formmatedHour).subscribe(() => {

        this.loadScheduleWeekDays(this.currentDay, this.currentHour);

        if (!this.isMemberPinned) {
          this.selectedMember = null;
        }

      });
  }

  onRemoveBookMark(userId) {


    const selDate = new Date(this.dailyRangeSchedule[this.currentDay].date);

    const month = '00' + (selDate.getMonth() + 1);
    const day = '00' + selDate.getDate();

    const formattedDate = selDate.getFullYear() + month.substring(month.length - 2) + day.substring(day.length - 2);

    const formmatedHour = this.dailyRangeSchedule[this.currentDay].selectableHours[this.currentHour].range.substring(0, 2)
      + this.dailyRangeSchedule[this.currentDay].selectableHours[this.currentHour].range.substring(3, 5);

    console.log(userId);
    this.gymService.deleteMemberDate(userId, formattedDate, formmatedHour).subscribe(() => {
      this.loadScheduleWeekDays(this.currentDay, this.currentHour);
    });

  }

}
