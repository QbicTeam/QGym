import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { ModalController } from '@ionic/angular';
import { BlockModalComponent } from '../block-modal/block-modal.component';

@Component({
  selector: 'app-blockusers',
  templateUrl: './blockusers.component.html',
  styleUrls: ['./blockusers.component.scss'],
})
export class BlockusersComponent implements OnInit {

  data: any[] = [];
  dataSearched: any[];

  constructor(private gymService: GymService, private modalCtrl: ModalController) { }

  ngOnInit() {

    this.data = this.gymService.getMembersList();
    this.dataSearched = this.data;

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

  async openModal(member) {
    const modal = await this.modalCtrl.create({
      component: BlockModalComponent,
      componentProps: { memberId: member.memberId, fullName: member.fullName, email: member.email, photoUrl: member.photoUrl }
    });

    await modal.present();
  }

}
