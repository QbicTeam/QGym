import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { ModalController } from '@ionic/angular';
import { UsersModalComponent } from '../users-modal/users-modal.component';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
})
export class UsersComponent implements OnInit {

  data: any[] = [];
  dataSearched: any[];

  constructor(private gymService: GymService, private modalCtrl: ModalController) { }

  ngOnInit() {
    this.data = this.gymService.getMembersList();
    this.dataSearched = this.data;
  }

  searchMember(event) {
    console.log(event.detail.value);

    const val = event.target.value;

    this.dataSearched = this.data;
    if (val && val.trim() !== '' ) {
      this.dataSearched = this.dataSearched.filter((item: any) => {
        return (item.fullName.toLowerCase().indexOf(val.toLowerCase()) > -1);
      });
    }

  }

  async openModal(member) {
    const modal = await this.modalCtrl.create({
      component: UsersModalComponent
    });

    await modal.present();
  }


}

