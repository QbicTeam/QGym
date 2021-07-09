import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-schedulesummary',
  templateUrl: './schedulesummary.component.html',
  styleUrls: ['./schedulesummary.component.scss'],
})
export class SchedulesummaryComponent implements OnInit {

  @Input() memberSchedule: any;
  scheduleCollapsed = false;
  hideMessage = false;

  constructor() { }

  ngOnInit() {
    console.log('summary',  this.memberSchedule);
    this.checkDisplayStatus();
  }

  onCollapse() {
      this.scheduleCollapsed = !this.scheduleCollapsed;
  }

  getDisplayMonth(month: number) {

    const months = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];

    return months[month];
  }

  getFormmattedDate(date) {

    const workDate = new Date(date);

    let result = '';

    const weekDays = ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'];

    result = result + weekDays[workDate.getDay()];
    result = result + ' ' + workDate.getDate();
    result = result + ' ' + this.getDisplayMonth(workDate.getMonth());
    // result = result + date.getYear();

    return result;
  }

  checkDisplayStatus() {
    const status = localStorage.getItem('scheduleDisplayStatus');

    if (status === 'true') {
      this.scheduleCollapsed = true;
      this.hideMessage = false;
    } else {
      this.scheduleCollapsed = false;
      this.hideMessage = true;
    }
  }

  onCheckStatus() {


    localStorage.setItem('scheduleDisplayStatus', this.hideMessage.toString());

  }

}
