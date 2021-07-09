import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { ModalController } from '@ionic/angular';
import { PackagesModalComponent } from '../packages-modal/packages-modal.component';
import { SharedService } from 'src/app/api/shared.service';

@Component({
  selector: 'app-gympackages',
  templateUrl: './gympackages.component.html',
  styleUrls: ['./gympackages.component.scss'],
})
export class GympackagesComponent implements OnInit {

  data: any;

  constructor(private gymService: GymService, private modalCtrl: ModalController, private sharedService: SharedService) { }

  ngOnInit() {

    this.loadPackagesList();

    this.sharedService.onUpdateData.subscribe(value => {
      if (value === 'packages') {
        this.loadPackagesList();
      }
    });
  }

  loadPackagesList() {

    this.gymService.getPackagesList().subscribe(response => {
      this.data = response;
      console.log('packages lists: ', this.data);
    });

  }

  async onAddNewPackage() {

    this.openModal({id: 0});

  }


  async openModal(pkg) {
    const modal = await this.modalCtrl.create({
      component: PackagesModalComponent,
      componentProps: { pkgId: pkg.id }
    });

    await modal.present();
  }

}
