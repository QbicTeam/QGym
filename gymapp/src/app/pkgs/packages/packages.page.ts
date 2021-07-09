import { Component, OnInit } from '@angular/core';
import { GymService } from 'src/app/api/gym.service';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { SecurityService } from 'src/app/api/security.service';

@Component({
  selector: 'app-packages',
  templateUrl: './packages.page.html',
  styleUrls: ['./packages.page.scss'],
})
export class PackagesPage implements OnInit {

  basePhotosUrl = environment.profilesPhotosRepoUrl + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/';

  currentMenu: any;
  currentUser: any;


  data: any;

  constructor(private gymService: GymService, private sanitazer: DomSanitizer,
              private router: Router, private securityService: SecurityService) { }

  ionViewDidEnter() {
    this.gymService.getPackagesListInfo().subscribe(response => {

      console.log('Getting package list....', response, this.securityService.getCurrentLoggedUser());

      const result = response;
      this.data = new Array<any>();

      for (const itm of result) {
        this.data.push(
          {
            id: itm.id,
            forSale: itm.forSale,
            price: itm.price,
            name: itm.name,
            period: itm.period,
            shortDescription: this.sanitazer.bypassSecurityTrustHtml(itm.shortDescription)
          }
        );
      }

    });

  }

  ngOnInit() {
    this.currentUser = this.securityService.getCurrentLoggedUser();
    this.currentMenu = this.securityService.getMenuByCurrentUserRole();

  }

  onMyProfile() {

  }

  onSelectedOption(option) {

    console.log(option);

    if (option === 'packages') {
      this.router.navigate(['/packages']);
    }
    else if (option === 'schedule') {
      this.router.navigate(['/schedule']);
    }
    else if (option === 'frontdesk') {
      this.router.navigate(['/frontdesk']);
    }
    else if (option === 'admin') {
      this.router.navigate(['/configuration']);
    }
    else if (option === 'sales') {
      this.router.navigate(['/sales-report']);
    }
  }

  logOut() {
    this.securityService.logOut();
    this.router.navigateByUrl('/home');
  }

}
