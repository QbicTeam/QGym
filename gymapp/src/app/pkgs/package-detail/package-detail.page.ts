import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { GymService } from 'src/app/api/gym.service';

@Component({
  selector: 'app-package-detail',
  templateUrl: './package-detail.page.html',
  styleUrls: ['./package-detail.page.scss'],
})
export class PackageDetailPage implements OnInit {

  data: any;

  constructor(private gymService: GymService, private sanitazer: DomSanitizer) { }

  ngOnInit() {
    this.data = this.sanitazer.bypassSecurityTrustHtml(this.gymService.getPackageDetails(123));
  }

}
