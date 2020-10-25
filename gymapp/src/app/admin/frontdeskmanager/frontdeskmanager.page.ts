import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { ModalController } from '@ionic/angular';
import { SelectmemberModalComponent } from '../selectmember-modal/selectmember-modal.component';
import { ThrowStmt } from '@angular/compiler';

@Component({
  selector: 'app-frontdeskmanager',
  templateUrl: './frontdeskmanager.page.html',
  styleUrls: ['./frontdeskmanager.page.scss'],
})
export class FrontdeskmanagerPage implements OnInit {

  currentDay = 0;
  dailyRangeSchedule: any;
  rangeHours: any;

  data: any[] = [];
  dataSearched: any[];
  selectedMember: any;
  isMemberPinned = false;


  constructor(private gymService: GymService, private modalCtrl: ModalController) { }

  ngOnInit() {
    this.dailyRangeSchedule = this.gymService.getRangeForDailySchedule();

    // this.data = this.gymService.getMembersDetailsList();
    // this.dataSearched = this.data;

  }

  goto(slideNumber) {
    // Get Available hour of selected date
    this.rangeHours = this.gymService.getHoursByDate();

    if (!this.isMemberPinned) {
      this.selectedMember = null;
    }
  }

  getDisplayMonth(month: number) {

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
      component: SelectmemberModalComponent
    });

    modal.onDidDismiss().then(resulData => {
      this.selectedMember = resulData.data;
      this.isMemberPinned = false;
    });

    await modal.present();
  }

  onPinMember() {
    this.isMemberPinned = !this.isMemberPinned;
  }


}
