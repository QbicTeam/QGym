import { Component, Input, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { ModalController } from '@ionic/angular';
import { BlockModalComponent } from '../block-modal/block-modal.component';
import { SharedService } from 'src/app/api/shared.service';

@Component({
  selector: 'app-blockusers',
  templateUrl: './blockusers.component.html',
  styleUrls: ['./blockusers.component.scss'],
})
export class BlockusersComponent implements OnInit {

  @Input() data: any;
  @Input() dataSearched: any;

  constructor(private gymService: GymService, private modalCtrl: ModalController, private sharedService: SharedService) { }

  ngOnInit() {

    // this.loadActiveMemberList();

  }

  // loadActiveMemberList() {

  //   this.gymService.getMembersList().subscribe(response => {

  //     console.log(response);

  //     this.data = response;
  //     this.dataSearched = this.data;

  //   });
  // }

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
