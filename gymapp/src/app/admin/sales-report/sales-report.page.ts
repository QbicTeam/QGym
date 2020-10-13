import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';

@Component({
  selector: 'app-sales-report',
  templateUrl: './sales-report.page.html',
  styleUrls: ['./sales-report.page.scss'],
})
export class SalesReportPage implements OnInit {

  data: any;

  constructor(private gymService: GymService) { }

  ngOnInit() {
    this.data = this.gymService.getSalesReport(null, null);
  }

}
