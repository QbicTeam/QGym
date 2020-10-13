import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-packages-modal',
  templateUrl: './packages-modal.component.html',
  styleUrls: ['./packages-modal.component.scss'],
})
export class PackagesModalComponent implements OnInit {

  constructor(private modalCtrl: ModalController) { }

  ngOnInit() {}

  dismissModal() {
    this.modalCtrl.dismiss();
  }

}
