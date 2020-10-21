import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-users-modal',
  templateUrl: './users-modal.component.html',
  styleUrls: ['./users-modal.component.scss'],
})
export class UsersModalComponent implements OnInit {

  uploader: FileUploader;
  baseUrl = environment.photosAPIUrl + 'photos/' + environment.profilesPhotosProjectName + '/' + environment.profilesPhotosFolderName + '/photos/';
  currentFileName = '';
  currentUserPhoto = '';

  constructor(private modalCtrl: ModalController) { }

  ngOnInit() {
    this.initializeUploader();
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
    const data = JSON.parse(response);

    console.log('photo uploaded success...', response);


    this.currentUserPhoto = environment.profilesPhotosRepoUrl +  environment.profilesPhotosProjectName
      + '/' + environment.profilesPhotosFolderName + '/' + data.url;
  }

  onUpload() {
    if (this.uploader.queue.length > 0) {
      this.uploader.uploadAll();
    } else {
      console.log('length = 0');
    }
  }

  onSaveProfileData() {
    console.log('saving....');
    this.onUpload();
  }
}
