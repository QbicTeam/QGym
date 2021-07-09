// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  // tslint:disable-next-line:max-line-length
  apiurl: 'http://modernfitnessgym.com/apis/gymapp_api/', // 'http://majahide-001-site1.itempurl.com/releasecandidates/GymAPI/', // 'http://modernfitnessgym.com/apis/gymapp_api/', // 'http://majahide-001-site1.itempurl.com/releasecandidates/GymAPI/',
  // tslint:disable-next-line:max-line-length
  photosAPIUrl: 'http://prometheusapis.net/_apis/PhotosAPI/', // 'http://majahide-001-site1.itempurl.com/releasecandidates/PhotosManagerAPI/',
  profilesPhotosRepoUrl:  'http://majahide-001-site1.itempurl.com/releasecandidates/PhotosManagerAPI/prometheusmedia/',
  profilesPhotosProjectName: 'gymfitness',
  profilesPhotosFolderName: 'profiles'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
