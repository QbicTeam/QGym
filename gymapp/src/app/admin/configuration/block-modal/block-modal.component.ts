import { Component, OnInit, Input } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { GymService } from 'src/app/api/gym.service';
import { SharedService } from 'src/app/api/shared.service';

@Component({
  selector: 'app-block-modal',
  templateUrl: './block-modal.component.html',
  styleUrls: ['./block-modal.component.scss'],
})
export class BlockModalComponent implements OnInit {

  @Input() memberId: any;
  @Input() fullName: any;
  @Input() email: any;
  @Input() photoUrl: any;
  memberInfo: any;

  reason = '';
  isBlocked = false;

  constructor(private modalCtrl: ModalController, private gymService: GymService, private sharedService: SharedService) { }

  ngOnInit() {
    this.loadMemberData();
  }

  loadMemberData() {
    this.gymService.getMemberDataForBlock(this.memberId).subscribe(response => {
      this.memberInfo = response;
      if (this.memberInfo.isBlock) {
        this.isBlocked = true;
      } else {
        this.isBlocked = false;
      }
    });
  }

  dismissModal() {
    this.modalCtrl.dismiss();
  }

  onChangeStatus() {

    this.gymService.updateMemberStatus(this.memberId, this.isBlocked, this.reason).subscribe(() => {
      this.sharedService.setConfigurationSelection('block');
    });

  }

}
