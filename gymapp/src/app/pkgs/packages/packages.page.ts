import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-packages',
  templateUrl: './packages.page.html',
  styleUrls: ['./packages.page.scss'],
})
export class PackagesPage implements OnInit {

  data: any;

  constructor(private gymService: GymService, private sanitazer: DomSanitizer) { }

  ngOnInit() {
    const result = this.gymService.getPackagesListInfo();
    this.data = new Array<any>();

    for (const itm of result) {
      this.data.push(
        {
          id: itm.id,
          price: itm.price,
          period: itm.period,
          shortDescription: this.sanitazer.bypassSecurityTrustHtml(itm.shortDescription)
        }
      );
    }
  }

}
