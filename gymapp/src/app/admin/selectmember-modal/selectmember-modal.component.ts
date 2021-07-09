import { Component, Input, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { GymService } from 'src/app/api/gym.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-selectmember-modal',
  templateUrl: './selectmember-modal.component.html',
  styleUrls: ['./selectmember-modal.component.scss'],
})
export class SelectmemberModalComponent implements OnInit {

  @Input() currentDate: any;
  data: any;
  dataSearched: any;
  basePhotosUrl = environment.profilesPhotosRepoUrl + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/';

  constructor(private gymService: GymService, private modalCtrl: ModalController) { }

  ngOnInit() {

    console.log(this.currentDate);

    this.gymService.getElelibleMembers(this.currentDate).subscribe(response => {
      console.log('elegibles', response);
      this.data = response;
      this.dataSearched = this.data;
    });

  }

  loadElegibleMembers() {

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
