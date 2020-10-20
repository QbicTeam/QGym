import { Component, Input, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { ModalController } from '@ionic/angular';
import { CapacityModalComponent } from '../capacity-modal/capacity-modal.component';

@Component({
  selector: 'app-capacity',
  templateUrl: './capacity.component.html',
  styleUrls: ['./capacity.component.scss'],
})
export class CapacityComponent implements OnInit {

  @Input() data: any;

  constructor(private gymService: GymService, private modalCtrl: ModalController) { }

  ngOnInit() {
  }

  calculateDiff(dueDate){
    const currentDate = new Date();
    dueDate = new Date(dueDate);

    // tslint:disable-next-line:max-line-length
    return Math.floor((Date.UTC(dueDate.getFullYear(), dueDate.getMonth(), dueDate.getDate()) - Date.UTC(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate())) / (1000 * 60 * 60 * 24));
  }

  onUpdateCapacity() {

    this.gymService.updateTotalCapacity(this.data.totalCapacity).subscribe(() => {
    });

  }

  async onAddNewCapacity() {
    const modal = await this.modalCtrl.create({
      component: CapacityModalComponent,
      componentProps: { totalCapacity: this.data.totalCapacity }
    });

    await modal.present();
  }

}
