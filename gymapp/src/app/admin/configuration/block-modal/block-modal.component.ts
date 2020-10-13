import { Component, OnInit, Input } from '@angular/core';
import { ModalController } from '@ionic/angular';

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

  reason = '';
  isBlocked = false;

  constructor(private modalCtrl: ModalController) { }

  ngOnInit() {}

  dismissModal() {
    this.modalCtrl.dismiss();
  }

}
