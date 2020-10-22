import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ModalController, AngularDelegate } from '@ionic/angular';
import { NgForm } from '@angular/forms';
import { GymService } from 'src/app/api/gym.service';
import { SharedService } from 'src/app/api/shared.service';

@Component({
  selector: 'app-capacity-modal',
  templateUrl: './capacity-modal.component.html',
  styleUrls: ['./capacity-modal.component.scss'],
})
export class CapacityModalComponent implements OnInit {

  @ViewChild('f', {static: true}) form: NgForm;
  @Input() totalCapacity: any;

  allowedPercentage = 0;
  allowedPeople = 0;
  todayDate = new Date();
  startDate: Date = this.todayDate;
  endDate: Date = this.startDate;

  constructor(private modalCtrl: ModalController, private gymService: GymService, private sharedService: SharedService) { }

  ngOnInit() {}

  dismissModal() {
    this.modalCtrl.dismiss();
  }

  onCalculatePeople() {
    console.log('sample');
    let result = 0;

    result = Math.round(this.totalCapacity * (this.allowedPercentage / 100));

    this.allowedPeople = result;
  }

  onSetStartDate() {

    const date1 = new Date(this.startDate);
    const date2 = new Date(this.endDate);

    if (date1 > date2) {
      this.endDate = this.startDate;
    }

  }

  onAddAuthorizedCapacity() {
    this.gymService.addAuthorizedCapacity(this.startDate, this.endDate, this.allowedPercentage, this.allowedPeople).subscribe(() => {
      this.sharedService.setConfigurationSelection('ocupation');
      this.modalCtrl.dismiss();
    });
  }
}
