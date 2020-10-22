import { Component, Input, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent implements OnInit {

  @Input() data: any;

  constructor(private gymService: GymService) { }

  ngOnInit() {}

  onUpdateConfiguration() {
    this.gymService.updateScheduleConfiguration(this.data).subscribe(() => {

    });
  }

}
