import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CovidMessageResponseDTO } from '../_models/CovidMessageResponseDTO';
import { DailyScheduleResponseDTO } from '../_models/DailyScheduleResponseDTO';
import { DownloadSalesReportResponseDTO } from '../_models/DownloadSalesReportReponseDTO';
import { PackageFullDescriptionResponseDTO } from '../_models/PackageFullDescriptionResponseDTO';

@Injectable({
  providedIn: 'root'
})
export class GymService {

  baseUrl = environment.apiurl;

  constructor(private http: HttpClient) { }

  getActiveMembersList() {

    const url = this.baseUrl + 'members/valid';

    return this.http.get(url);
  }

  getMembersList() {

    const url = this.baseUrl + 'members';

    return this.http.get(url);
  }

  getMemberDataForBlock(memberId: string) {
    const url = this.baseUrl + 'members/' + memberId + '/details/forBlock';

    return this.http.get(url);
  }

  updateMemberStatus(memberId: string, bloqued: boolean, reason: string) {

    const url = this.baseUrl + 'members/' + memberId + '/status';

    const requestData = {
      isBlock: !bloqued,
      blockingReason: reason
    };

    return this.http.put(url, requestData);
  }

  getCapacityData() {

    const url = this.baseUrl + 'capacity';

    return this.http.get(url);
  }

  updateTotalCapacity(totalCapacity: any) {

    const url = this.baseUrl + 'capacity/' + totalCapacity;

    return this.http.put(url, null);
  }

  addAuthorizedCapacity(startDate, endDate, percentage, people) {

    const url = this.baseUrl + 'capacity/autorizedCapacities';

    const requestData = {
      startDate,
      endDate,
      capacity: people,
      percentageCapacity: percentage
    };

    return this.http.post(url, requestData);

  }

  getScheduleConfiguration() {

    const url = this.baseUrl + 'schedule';

    return this.http.get(url);
  }

  getGeneralSettingsConfiguration() {
    const url = this.baseUrl + 'admin/settings/general';

    return this.http.get(url);
  }

  updateScheduleConfiguration(previousHours: any) {

    const url = this.baseUrl + 'schedule/' + previousHours;

    return this.http.put(url, null);

  }

  updateGeneralSettingConfiguration(dataToSave: any) {

    const url = this.baseUrl + 'admin/settings/general';

    return this.http.put(url, dataToSave);

  }

  getMemberDetailsById(id) {

    const url = this.baseUrl + 'members/' + id + '/details/complete';

    return this.http.get(url);
  }

  updateMemberData(memberData: any) {

    const url = this.baseUrl + 'members/' + memberData.userId;

    return this.http.put(url, memberData);

  }

  getPackagesList() {

    const url = this.baseUrl + 'packages';

    return this.http.get(url);

  }

  getPackageFullDetailsById(pkgId: any) {

    const url = this.baseUrl + 'packages/' + pkgId;

    return this.http.get(url);
  }

  updatePackageById(pkgId: any, pkg: any) {

    const url = this.baseUrl + 'packages/' + pkgId;

    return this.http.put(url, pkg);

  }

  addNewPackage(pkg: any) {

    const url = this.baseUrl + 'packages';

    return this.http.post(url, pkg);

  }

  updateMemberId(userId: number, memberId: number, force: boolean) {

    let url = this.baseUrl + 'members/' + userId + '/' + memberId;

    if (force) {
      url = url + '/force';
    }

    return this.http.put(url, null);
  }





  getMemberSchedule(memberId: number) {

    const data = [
      {
        date: new Date(2020, 8, 28),
        bookedHour: '09:00 - 10:00',
        selectableHours: [ { range: '09:00 - 10:00', booked: true },
          { range: '10:00 - 11:00' }, { range: '11:00 - 12:00' }, { range: '12:00 - 13:00' },
          { range: '13:00 - 14:00' }, { range: '14:00 - 14:00' }, { range: '15:00 - 16:00' },
          { range: '16:00 - 17:00' }, { range: '17:00 - 18:00' }, { range: '18:00 - 19:00' }
        ]
      },
      {
        date: new Date(2020, 8, 29),
        bookedHour: '11:00 - 12:00',
        selectableHours: [ { range: '09:00 - 10:00' },
          { range: '10:00 - 11:00' }, { range: '11:00 - 12:00', capacity: 75, booked: true }, { range: '12:00 - 13:00' },
          { range: '13:00 - 14:00' }, { range: '14:00 - 14:00' }, { range: '15:00 - 16:00' },
          { range: '16:00 - 17:00' }, { range: '17:00 - 18:00', capacity: 100 }, { range: '18:00 - 19:00' }
        ]
      },
      {
        date: new Date(2020, 8, 30),
        bookedHour: '09:00 - 10:00',
        selectableHours: [ { range: '09:00 - 10:00', booked: true },
          { range: '10:00 - 11:00' }, { range: '11:00 - 12:00' }, { range: '12:00 - 13:00' },
          { range: '13:00 - 14:00' }, { range: '14:00 - 14:00' }, { range: '15:00 - 16:00' }
        ]
      },
      {
        date: new Date(2020, 9, 1),
        bookedHour: '',
        selectableHours: [ { range: '09:00 - 10:00' },
          { range: '10:00 - 11:00' }, { range: '11:00 - 12:00' }, { range: '12:00 - 13:00' },
          { range: '13:00 - 14:00' }, { range: '14:00 - 14:00' }, { range: '15:00 - 16:00' },
          { range: '16:00 - 17:00' }, { range: '17:00 - 18:00' }, { range: '18:00 - 19:00' }
        ]
      },
      {
        date: new Date(2020, 9, 2),
        bookedHour: '12:00 - 13:00',
        selectableHours: [ { range: '09:00 - 10:00' },
          { range: '10:00 - 11:00' }, { range: '11:00 - 12:00' }, { range: '12:00 - 13:00', booked: true },
          { range: '13:00 - 14:00' }, { range: '14:00 - 14:00' }, { range: '15:00 - 16:00' },
          { range: '16:00 - 17:00' }, { range: '17:00 - 18:00' }, { range: '18:00 - 19:00' }
        ]
      },
      {
        date: new Date(2020, 9, 3),
        bookedHour: '14:00 - 15:00',
        selectableHours: [ { range: '09:00 - 10:00' },
          { range: '10:00 - 11:00' }, { range: '11:00 - 12:00' }, { range: '12:00 - 13:00' },
          { range: '13:00 - 14:00' }, { range: '14:00 - 14:00', capacity: 100, booked: true }, { range: '15:00 - 16:00' },
          { range: '16:00 - 17:00' }, { range: '17:00 - 18:00' }, { range: '18:00 - 19:00' }
        ]
      },
      {
        date: new Date(2020, 9, 4),
        bookedHour: '',
        selectableHours: [ { range: '09:00 - 10:00' },
          { range: '10:00 - 11:00' }, { range: '11:00 - 12:00' }, { range: '12:00 - 13:00' },
          { range: '13:00 - 14:00' }, { range: '14:00 - 14:00', capacity: 83 }, { range: '15:00 - 16:00' },
          { range: '16:00 - 17:00' }, { range: '17:00 - 18:00' }, { range: '18:00 - 19:00' }
        ]
      }
    ];

    return data;

  }

  getGeneralSchedule() {

    const data = [
      {
        date: new Date(2020, 8, 28),
        selectableHours: [ { range: '09:00 - 10:00', capPercentaje: 10, capPeople: 10 },
          { range: '10:00', capPercentaje: 31, capPeople: 31 },
          { range: '11:00', capPercentaje: 51, capPeople: 51 },
          { range: '12:00', capPercentaje: 84, capPeople: 84 },
          { range: '13:00', capPercentaje: 63, capPeople: 63 },
          { range: '14:00', capPercentaje: 96, capPeople: 96 },
          { range: '15:00', capPercentaje: 100, capPeople: 100 },
          { range: '16:00', capPercentaje: 69, capPeople: 69 },
          { range: '17:00', capPercentaje: 80, capPeople: 80 },
          { range: '18:00', capPercentaje: 47, capPeople: 47 }
        ]
      },
      {
        date: new Date(2020, 8, 29),
        selectableHours: [ { range: '09:00', capPercentaje: 5, capPeople: 5 },
          { range: '10:00', capPercentaje: 35, capPeople: 35 },
          { range: '11:00', capPercentaje: 100, capPeople: 100 },
          { range: '12:00', capPercentaje: 77, capPeople: 77 },
          { range: '13:00', capPercentaje: 87, capPeople: 87 },
          { range: '14:00', capPercentaje: 86, capPeople: 86 },
          { range: '15:00', capPercentaje: 80, capPeople: 80 },
          { range: '16:00', capPercentaje: 10, capPeople: 10 },
          { range: '17:00', capPercentaje: 100, capPeople: 100 },
          { range: '18:00', capPercentaje: 69, capPeople: 69 }
        ]
      },
      {
        date: new Date(2020, 8, 30),
        selectableHours: [ { range: '09:00', capPercentaje: 32, capPeople: 32 },
          { range: '10:00', capPercentaje: 17, capPeople: 17 },
          { range: '11:00', capPercentaje: 10, capPeople: 10 },
          { range: '12:00', capPercentaje: 96, capPeople: 96 },
          { range: '13:00', capPercentaje: 100, capPeople: 100 },
          { range: '14:00', capPercentaje: 39, capPeople: 39 },
          { range: '15:00', capPercentaje: 6, capPeople: 6 }
        ]
      },
      {
        date: new Date(2020, 9, 1),
        selectableHours: [ { range: '09:00', capPercentaje: 21, capPeople: 21 },
          { range: '10:00', capPercentaje: 62, capPeople: 62 },
          { range: '11:00', capPercentaje: 73, capPeople: 73 },
          { range: '12:00', capPercentaje: 0, capPeople: 0 },
          { range: '13:00', capPercentaje: 16, capPeople: 16 },
          { range: '14:00', capPercentaje: 22, capPeople: 22 },
          { range: '15:00', capPercentaje: 2, capPeople: 2 },
          { range: '16:00', capPercentaje: 12, capPeople: 12 },
          { range: '17:00', capPercentaje: 86, capPeople: 86 },
          { range: '18:00', capPercentaje: 100, capPeople: 100 }
        ]
      },
      {
        date: new Date(2020, 9, 2),
        selectableHours: [ { range: '09:00', capPercentaje: 73, capPeople: 73 },
          { range: '10:00', capPercentaje: 52, capPeople: 52 },
          { range: '11:00', capPercentaje: 43, capPeople: 43 },
          { range: '12:00', capPercentaje: 43, capPeople: 43 },
          { range: '13:00', capPercentaje: 97, capPeople: 97 },
          { range: '14:00', capPercentaje: 0, capPeople: 0 },
          { range: '15:00', capPercentaje: 26, capPeople: 26 },
          { range: '16:00', capPercentaje: 30, capPeople: 30 },
          { range: '17:00', capPercentaje: 77, capPeople: 77 },
          { range: '18:00', capPercentaje: 87, capPeople: 87 }
        ]
      },
      {
        date: new Date(2020, 9, 3),
        selectableHours: [ { range: '09:00', capPercentaje: 38, capPeople: 38 },
          { range: '10:00', capPercentaje: 100, capPeople: 100 },
          { range: '11:00', capPercentaje: 63, capPeople: 63 },
          { range: '12:00', capPercentaje: 42, capPeople: 42 },
          { range: '13:00', capPercentaje: 92, capPeople: 92 },
          { range: '14:00', capPercentaje: 47, capPeople: 47 },
          { range: '15:00', capPercentaje: 41, capPeople: 41 },
          { range: '16:00', capPercentaje: 0, capPeople: 0 },
          { range: '17:00', capPercentaje: 6, capPeople: 6 },
          { range: '18:00', capPercentaje: 37, capPeople: 37 }
        ]
      },
      {
        date: new Date(2020, 9, 4),
        selectableHours: [ { range: '09:00', capPercentaje: 21, capPeople: 21 },
          { range: '10:00', capPercentaje: 56, capPeople: 56 },
          { range: '11:00', capPercentaje: 31, capPeople: 31 },
          { range: '12:00', capPercentaje: 100, capPeople: 100 },
          { range: '13:00', capPercentaje: 15, capPeople: 15 },
          { range: '14:00', capPercentaje: 97, capPeople: 97 },
          { range: '15:00', capPercentaje: 52, capPeople: 52 },
          { range: '16:00', capPercentaje: 7, capPeople: 7 },
          { range: '17:00', capPercentaje: 18, capPeople: 18 },
          { range: '18:00', capPercentaje: 0, capPeople: 0 }
        ]
      }
    ];

    return data;

  }

  getRangeForDailySchedule() {

    const data = [
        new Date(2020, 8, 28),
        new Date(2020, 8, 29),
        new Date(2020, 8, 30),
        new Date(2020, 9, 1),
        new Date(2020, 9, 2),
        new Date(2020, 9, 3),
        new Date(2020, 9, 4),
    ];

    return data;
  }

  getHoursByDate() {

    const data = [
      '09:00 - 10:00',
      '10:00 - 11:00',
      '11:00 - 12:00',
      '12:00 - 13:00',
      '13:00 - 14:00',
      '14:00 - 15:00',
      '15:00 - 16:00',
      '16:00 - 17:00',
      '17:00 - 18:00',
      '18:00 - 19:00',
    ];

    return data;

  }

  getPackagesListInfo(): Observable<Array<any>> {

    const url = this.baseUrl + 'packages/actives';

    return this.http.get<Array<any>>(url);

  }

  getPackageDetails(idPackage: number): Observable<PackageFullDescriptionResponseDTO> {

    const url = this.baseUrl + 'packages/' + idPackage + '/fullDescription';

    return this.http.get<PackageFullDescriptionResponseDTO>(url);
  }

  getSalesReport(initDate: Date, endDate: Date) {

    const startMonth = '00' + initDate.getMonth();
    const startDay = '00' + initDate.getDate();

    const endMonth = '00' + endDate.getMonth();
    const endDay = '00' + endDate.getDate();

    const startDate = initDate.getFullYear() + startMonth.substring(startMonth.length - 2) + startDay.substring(startDay.length - 2);
    const finalDate = endDate.getFullYear() + endMonth.substring(endMonth.length - 2) + endDay.substring(endDay.length - 2);


    const url = this.baseUrl + 'sales/report/' + startDate + '/' + finalDate;

    return this.http.get(url);
  }

  getSalesReportForDownload(initDate: Date, endDate: Date): Observable<DownloadSalesReportResponseDTO> {

    const startMonth = '00' + initDate.getMonth();
    const startDay = '00' + initDate.getDate();

    const endMonth = '00' + endDate.getMonth();
    const endDay = '00' + endDate.getDate();

    const startDate = initDate.getFullYear() + startMonth.substring(startMonth.length - 2) + startDay.substring(startDay.length - 2);
    const finalDate = endDate.getFullYear() + endMonth.substring(endMonth.length - 2) + endDay.substring(endDay.length - 2);


    const url = this.baseUrl + 'sales/report/' + startDate + '/' + finalDate + '/download';


    return this.http.get<DownloadSalesReportResponseDTO>(url);

  }

  getCovidMessage(): Observable<CovidMessageResponseDTO> {

    const url = this.baseUrl + 'admin/settings/CovidMsg';

    return this.http.get<CovidMessageResponseDTO>(url);

  }

  getMemberScheduleSummaryByUserId(userId) {

    const url = this.baseUrl + 'members/' + userId + '/schedule/summary';

    return this.http.get(url);

  }

  getScheduleWeekly(userId) {

    const url = this.baseUrl + 'members/' + userId + '/schedule/weekly';

    return this.http.get(url);

  }

  getFrontDeskScheduleWeekly() {

    const url = this.baseUrl + 'admin/schedule/weekly';

    return this.http.get(url);

  }


  getScheduleByDayAndUser(userId: any, date: any): Observable<DailyScheduleResponseDTO> {

    const selDate = new Date(date);

    const month = '00' + (selDate.getMonth() + 1);
    const day = '00' + selDate.getDate();

    const formattedDate = selDate.getFullYear() + month.substring(month.length - 2) + day.substring(day.length - 2);

    const url = this.baseUrl + 'members/' + userId + '/schedule/' + formattedDate;

    return this.http.get<DailyScheduleResponseDTO>(url);

  }

  bookMemberDate(userId: any, date: any, hour: any) {

    const url = this.baseUrl + 'members/' + userId + '/schedule';

    const requestData = {
      date,
      hour
    };

    console.log(requestData);

    return this.http.post(url, requestData);

  }

  deleteMemberDate(userId: any, date: any, hour: any) {

    const url = this.baseUrl + 'members/' + userId + '/schedule/' + date + '/' + hour;

    console.log(url);

    return this.http.delete(url);

  }

  getBookedMembers(date: Date, hour: string) {

    const selDate = new Date(date);

    const month = '00' + (selDate.getMonth() + 1);
    const day = '00' + selDate.getDate();

    const formattedDate = selDate.getFullYear() + month.substring(month.length - 2) + day.substring(day.length - 2);

    const formmatedHour = hour.substring(0, 2) + hour.substring(3, 5);

    const url = this.baseUrl + 'admin/schedule/' + formattedDate + '/' + formmatedHour + '/booked/members ';

    console.log(url);

    return this.http.get(url);


  }

  getElelibleMembers(date) {

    const selDate = new Date(date);

    const month = '00' + (selDate.getMonth() + 1);
    const day = '00' + selDate.getDate();

    const formattedDate = selDate.getFullYear() + month.substring(month.length - 2) + day.substring(day.length - 2);

    const url = this.baseUrl + 'admin/members/' + formattedDate + '/elegibles';

    return this.http.get(url);

  }

  payMembership(userId, pkgId, price) {

    const requestData = {
      MemberschipTypeId: pkgId,
      Amount: price
    };

    const url = this.baseUrl + 'Payment/' + userId;

    return this.http.post(url, requestData);


  }

}
