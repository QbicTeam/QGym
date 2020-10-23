import { Component, Input, OnInit } from '@angular/core';
import { LoadingController, ModalController } from '@ionic/angular';
import { FileUploader } from 'ng2-file-upload';
import { GymService } from 'src/app/api/gym.service';
import { SharedService } from 'src/app/api/shared.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-users-modal',
  templateUrl: './users-modal.component.html',
  styleUrls: ['./users-modal.component.scss'],
})
export class UsersModalComponent implements OnInit {

  @Input() userId: any;

  isMemberIdEditable = false;
  uploader: FileUploader;
  baseUrl = environment.photosAPIUrl + 'photos/' + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/photos/';
  currentFileName = '';
  currentUserPhoto = '';
  currentUser: any;
  memberDetails: any;
  // tslint:disable-next-line:max-line-length
  sourcePhotosPath = environment.profilesPhotosRepoUrl + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/';

  constructor(private modalCtrl: ModalController, private loadingCtrl: LoadingController,
              private gymService: GymService, private sharedService: SharedService) { }

  ngOnInit() {
    this.initializeUploader();
    this.loadMemberData();
  }

  loadMemberData() {
    console.log(this.userId);
    this.gymService.getMemberDetailsById(this.userId).subscribe(response => {
      console.log('member details...', response);
      this.memberDetails = response;

      if (this.memberDetails.memberId === '') {
        this.isMemberIdEditable = true;
      }

      this.currentUserPhoto = this.sourcePhotosPath + this.memberDetails.photoUrl;
    });
  }

  dismissModal() {
    this.modalCtrl.dismiss();
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl,
      authToken: 'Bearer ' + localStorage.getItem('gymToken'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
      this.currentFileName = file.file.name;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => this.onSuccessPhoto(item, response, status, headers);

  }

  onSuccessPhoto(item: any, response: any, status: any, headers: any) {

    this.loadingCtrl.getTop().then(hasLoading => {
      if (hasLoading) {
          this.loadingCtrl.dismiss();
      }
    });

    const data = JSON.parse(response);

    this.currentUserPhoto = environment.profilesPhotosRepoUrl +  environment.profilesPhotosProjectName
      + '/' + environment.profilesPhotosFolderName + '/' + data.url;

    this.currentFileName = data.url;

    this.saveProfileData();
  }

  onUpload() {
    if (this.uploader.queue.length > 0) {

      this.loadingCtrl.create({
        spinner: 'circular',
        translucent: true
    }).then(loading => loading.present());


      this.uploader.uploadAll();
    }
  }

  onSaveProfileData() {

    if (this.currentFileName && this.currentFileName !== '') {
      console.log('saving with photo.......');
      this.onUpload();
    } else {
      this.saveProfileData();
    }
  }

  saveProfileData() {
    console.log('saving profile details...');

    if (this.currentFileName && this.currentFileName !== '') {
      this.memberDetails.photoUrl = this.currentFileName;
    }

    this.gymService.updateMemberData(this.memberDetails).subscribe(() => {
      console.log('saved success.. ');
      this.sharedService.setOnUpdateData('members');
      this.currentFileName = '';

      if (this.memberDetails.memberId !== '') {
        this.isMemberIdEditable = false;
      }
    });

  }
}
