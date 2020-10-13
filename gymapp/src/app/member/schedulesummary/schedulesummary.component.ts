import { Component, OnInit, Input } from '@angular/core';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-schedulesummary',
  templateUrl: './schedulesummary.component.html',
  styleUrls: ['./schedulesummary.component.scss'],
})
export class SchedulesummaryComponent implements OnInit {

  @Input() memberSchedule: any;
  scheduleCollapsed = false;

  constructor(private modalCtrl: ModalController) { }

  ngOnInit() {}

  onCollapse() {
      this.scheduleCollapsed = !this.scheduleCollapsed;
  }

  getDisplayMonth(month: number) {

    const months = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];

    return months[month];
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

}
