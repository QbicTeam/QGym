import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
// import { File } from '@ionic-native/file';

@Component({
  selector: 'app-sales-report',
  templateUrl: './sales-report.page.html',
  styleUrls: ['./sales-report.page.scss'],
})
export class SalesReportPage implements OnInit {

  downloadUrl: any;
  startDate = new Date();
  endDate = new Date();
  downloadLink: any;

  data: any;

  constructor(private gymService: GymService) { }

  ngOnInit() {
  }

  ionViewDidEnter() {

    this.downloadLink = document.getElementById('downlodSalesReportFile') as HTMLElement;

    console.log(this.downloadLink);

    this.gymService.getSalesReport(this.startDate, this.endDate).subscribe(response => {
      console.log(response);
      this.data = response;
    });

  }

  onGenerateReport() {

    this.gymService.getSalesReportForDownload(this.startDate, this.endDate).subscribe(response => {
      console.log('link settted...');
      this.downloadUrl = response.url;
      this.downloadLink.click();
    });

  }
}
