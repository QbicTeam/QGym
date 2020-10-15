import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { ModalController } from '@ionic/angular';
import { PackagesModalComponent } from '../packages-modal/packages-modal.component';

@Component({
  selector: 'app-gympackages',
  templateUrl: './gympackages.component.html',
  styleUrls: ['./gympackages.component.scss'],
})
export class GympackagesComponent implements OnInit {

  data: any[];

  constructor(private gymService: GymService, private modalCtrl: ModalController) { }

  ngOnInit() {
    this.data = this.gymService.getPackagesList();
  }

  async onAddNewPackage() {

    const modal = await this.modalCtrl.create({
      component: PackagesModalComponent
    });

    await modal.present();

  }

}
