import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GymService } from 'src/app/api/gym.service';
import { SecurityService } from 'src/app/api/security.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-sales-report',
  templateUrl: './sales-report.page.html',
  styleUrls: ['./sales-report.page.scss'],
})
export class SalesReportPage implements OnInit {

  basePhotosUrl = environment.profilesPhotosRepoUrl + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/';
  currentMenu: any;
  currentUser: any;


  downloadUrl: any;
  startDate = new Date();
  endDate = new Date();

  data: any;

  constructor(private gymService: GymService, private securityService: SecurityService, private router: Router) { }

  ngOnInit() {

    this.currentUser = this.securityService.getCurrentLoggedUser();
    this.currentMenu = this.securityService.getMenuByCurrentUserRole();

  }

  ionViewDidEnter() {


    this.gymService.getSalesReport(this.startDate, this.endDate).subscribe(response => {
      console.log(response);
      this.data = response;
    });

  }

  onGenerateReport() {

    this.downloadUrl = null;

    this.gymService.getSalesReportForDownload(this.startDate, this.endDate).subscribe(response => {
      this.downloadUrl = response.url;
    });

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
    else if (option === 'activation') {
      this.router.navigate(['/membersactivation']);
    }
  }

  logOut() {
    this.securityService.logOut();
    this.router.navigateByUrl('/home');
  }

}
