import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { GymService } from 'src/app/api/gym.service';

@Component({
  selector: 'app-selectmember-modal',
  templateUrl: './selectmember-modal.component.html',
  styleUrls: ['./selectmember-modal.component.scss'],
})
export class SelectmemberModalComponent implements OnInit {

  data: any[] = [];
  dataSearched: any[];

  constructor(private gymService: GymService, private modalCtrl: ModalController) { }

  ngOnInit() {

    // this.data = this.gymService.getMembersDetailsList();
    // this.dataSearched = this.data;

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

  onMemberSelected(member) {
    this.modalCtrl.dismiss(member);
  }

}
