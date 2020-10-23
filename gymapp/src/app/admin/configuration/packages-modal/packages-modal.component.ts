import { ThrowStmt } from '@angular/compiler';
import { Component, Input, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { GymService } from 'src/app/api/gym.service';
import { SharedService } from 'src/app/api/shared.service';

@Component({
  selector: 'app-packages-modal',
  templateUrl: './packages-modal.component.html',
  styleUrls: ['./packages-modal.component.scss'],
})
export class PackagesModalComponent implements OnInit {

  @Input() pkgId: any;
  currentPkg: any;

  constructor(private modalCtrl: ModalController, private gymService: GymService, private sharedService: SharedService) { }

  ngOnInit() {
    console.log('PKG ID: ', this.pkgId);

    if (this.pkgId > 0) {
      this.loadPackageDetails(this.pkgId);
    } else {
      this.initNewPackage();
    }
  }

  initNewPackage() {
    this.currentPkg = {
      code: '',
      name: '',
      isActive: false,
      longDescription: '',
      periodicityDays: 0,
      price: 0,
      shortDescription: ''
    };
  }

  loadPackageDetails(pkgId) {

    this.gymService.getPackageFullDetailsById(pkgId).subscribe(response => {
      console.log(response);
      this.currentPkg = response;
    });

  }

  dismissModal() {
    this.modalCtrl.dismiss();
  }

  onSavePackage() {

    if (this.pkgId > 0) {
      this.gymService.updatePackageById(this.pkgId, this.currentPkg).subscribe(() => {
        this.sharedService.setOnUpdateData('packages');
      });
    } else {
      this.gymService.addNewPackage(this.currentPkg).subscribe(response => {
        console.log(response);
        this.pkgId = response;
        this.sharedService.setOnUpdateData('packages');
      });
    }
  }
}
