import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { GymService } from 'src/app/api/gym.service';
import { SecurityService } from 'src/app/api/security.service';

@Component({
  selector: 'app-package-detail',
  templateUrl: './package-detail.page.html',
  styleUrls: ['./package-detail.page.scss'],
})
export class PackageDetailPage implements OnInit {

  data: any;
  currentPkg: any;
  currentUser: any;

  constructor(private gymService: GymService, private sanitazer: DomSanitizer,
              private activatedRoute: ActivatedRoute, private securityService: SecurityService) { }

  ngOnInit() {

    this.currentUser = this.securityService.getCurrentLoggedUser();

    this.activatedRoute.paramMap.subscribe(paramMap => {

      if (paramMap.has('pkgid')) {


      this.currentPkg = {
        id: +paramMap.get('pkgid'),
        name: paramMap.get('name'),
        price: +paramMap.get('price')
        };

      this.gymService.getPackageDetails(this.currentPkg.id).subscribe(response => {
          this.currentPkg.forSale = response.forSale;
          this.data = this.sanitazer.bypassSecurityTrustHtml(response.longDescription);
        });

      }
    });

  }

}
