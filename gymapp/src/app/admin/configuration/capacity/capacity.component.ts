import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { ModalController } from '@ionic/angular';
import { CapacityModalComponent } from '../capacity-modal/capacity-modal.component';

@Component({
  selector: 'app-capacity',
  templateUrl: './capacity.component.html',
  styleUrls: ['./capacity.component.scss'],
})
export class CapacityComponent implements OnInit {

  data: any[];
  totalCapacity = 100;

  constructor(private gymService: GymService, private modalCtrl: ModalController) { }

  ngOnInit() {

    this.data = this.gymService.getCapacityList();
  }

  async onAddNewCapacity() {
    const modal = await this.modalCtrl.create({
      component: CapacityModalComponent,
      componentProps: { totalCapacity: this.totalCapacity }
    });

    await modal.present();
  }

}
