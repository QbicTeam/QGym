import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { ModalController } from '@ionic/angular';
import { UsersModalComponent } from '../users-modal/users-modal.component';
import { environment } from 'src/environments/environment';
import { SharedService } from 'src/app/api/shared.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
})
export class UsersComponent implements OnInit {

  data: any;
  dataSearched: any;
  basePhotosUrl = environment.profilesPhotosRepoUrl + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/';


  constructor(private gymService: GymService, private modalCtrl: ModalController, private sharedService: SharedService) { }

  ngOnInit() {
    this.loadMembersList();

    this.sharedService.onUpdateData.subscribe(value => {
      if (value === 'members') {
        console.log('member updated...');
        this.loadMembersList();
      }
    });
  }

  loadMembersList() {
    this.gymService.getMembersList().subscribe(response => {
      console.log('Users list members retrieved...');
      this.data = response;
      this.dataSearched = this.data;
    });
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
      component: UsersModalComponent,
      componentProps: { userId: member.userId }
    });

    await modal.present();
  }


}
