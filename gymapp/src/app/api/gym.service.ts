import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GymService {

  baseUrl = environment.apiurl;

  constructor(private http: HttpClient) { }

  getMembersList() {

    const url = this.baseUrl + 'members/valid';

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

  updateScheduleConfiguration(previousHours: any) {

    const url = this.baseUrl + 'schedule/' + previousHours;

    return this.http.put(url, null);

  }

  getMembersDetailsList() {

    const data = [
      {
        memberId: 12345,
        fullName: 'Camila Sodi',
        email: 'camilasodi@gmail.com',
        package: 'Familiar',
        period: '12 days',
        dueDate: 'Nov 12, 2020',
        photoUrl: 'https://image.shutterstock.com/image-photo/beauty-asian-woman-face-portrait-260nw-607071935.jpg',
        searchText: '12345 Camila Sodi camilasodi@gmail.com'
      },
      {
        memberId: 67890,
        fullName: 'Emma artenton',
        email: 'emma@hotmail.com',
        package: 'Personal',
        period: '20 days',
        dueDate: 'Oct 12, 2020',
        photoUrl: 'https://thumbs.dreamstime.com/b/beauty-woman-face-portrait-beautiful-spa-model-girl-perfect-fresh-clean-skin-youth-skin-care-concept-brunette-female-63494003.jpg',
        searchText: '67890 Emma artenton emma@hotmail.com'
      },
      {
        memberId: 24680,
        fullName: 'Anne Hateway',
        email: 'anne@gmail.com',
        package: 'Femenil',
        period: '9 days',
        dueDate: 'Sep 20, 2020',
        photoUrl: 'https://banner2.cleanpng.com/20180723/lho/kisspng-plastic-surgery-cosmetics-woman-face-skin-care-model-5b5605a8b02fd8.3457753715323642007217.jpg',
        searchText: '24680 Anne Hateway anne@gmail.com'
      },
    ];

    return data;

  }


  getCapacityList() {

    const data = [
      {
        startDate: '01/Jun/2020',
        endDate: '01/Ago/2020',
        percentage: 30,
        totalUsers: 40,
        remainingDays: -20
      },
      {
        startDate: '02/Ago/2020',
        endDate: '01/Dic/2020',
        percentage: 50,
        totalUsers: 80,
        remainingDays: 100
      }

    ];

    return data;
  }

  getPackagesList() {
    const data = [
      {
        packageId: 12345,
        name: 'Familiar',
        period: '7 Dias',
        isActive: true
      },
      {
        packageId: 67890,
        name: 'Especial',
        period: '100 Dias',
        isActive: false
      }
    ];

    return data;
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

  getPackagesListInfo() {

    const data = [
      {
        id: 12345,
        price: 120,
        period: '10 dias',
        shortDescription: '<div style="position: relative; text-align: center; color: #000;"><img src="../../assets/images/slider-accesototal.jpg"><div style="position: absolute; bottom: 40px; left: 16px; font-size: 38px; font-weight: 700; font-family: \'Montserrat\';">Acceso Total</div><div style="position: absolute; bottom: 100px; left: 16px; font-size: 30px; font-weight: 500; font-family: \'Montserrat\';">PLAN MENSUAL</div><div><a style="position: absolute; bottom: 150px; left: 16px;" target="_blank" href="https://www.redalyc.org/pdf/1794/179454112005.pdf">Descargar</a></div></div>'
      },
      {
        id: 23456,
        price: 60,
        period: '20 dias',
        shortDescription: '<div style="position: relative; text-align: center; color: #000;"><img src="../../assets/images/slider-estudiante.jpg"><div style="position: absolute; bottom: 40px; left: 16px; font-size: 38px; font-weight: 700; font-family: \'Montserrat\';">Estudiante</div><div style="position: absolute; bottom: 100px; left: 16px; font-size: 30px; font-weight: 500; font-family: \'Montserrat\';">PLAN MENSUAL</div><div><a style="position: absolute; bottom: 150px; left: 16px;" target="_blank" href="https://www.redalyc.org/pdf/206/20611455003.pdf">Descargar</a></div></div>'
      },
      {
        id: 34567,
        price: 34,
        period: '45 dias',
        shortDescription: '<div style="position: relative; text-align: center; color: #000;"><img src="../../assets/images/slider-familiar.jpg"><div style="position: absolute; bottom: 40px; left: 16px; font-size: 38px; font-weight: 700; font-family: \'Montserrat\';">Familiar</div><div style="position: absolute; bottom: 100px; left: 16px; font-size: 30px; font-weight: 500; font-family: \'Montserrat\';">PLAN MENSUAL</div></div>'
      },
      {
        id: 45678,
        price: 45,
        period: '43 dias',
        shortDescription: '<div style="position: relative; text-align: center; color: #000;"><img src="../../assets/images/slider-ladyfitness.jpg"><div style="position: absolute; bottom: 40px; left: 16px; font-size: 38px; font-weight: 700; font-family: \'Montserrat\';">Lady Fitness</div><div style="position: absolute; bottom: 100px; left: 16px; font-size: 30px; font-weight: 500; font-family: \'Montserrat\';">PLAN MENSUAL</div></div>'
      },
      {
        id: 56789,
        price: 450,
        period: '100 dias',
        shortDescription: '<div style="position: relative; text-align: center; color: #000;"><img src="../../assets/images/plan-ejecutivo.jpg"><div style="position: absolute; bottom: 40px; left: 16px; font-size: 38px; font-weight: 700; font-family: \'Montserrat\';">Plan Ejecutivo</div><div style="position: absolute; bottom: 100px; left: 16px; font-size: 30px; font-weight: 500; font-family: \'Montserrat\';">PLAN MENSUAL</div></div>'
      }
    ];


    return data;

  }

  getPackageDetails(idPackage: number) {

    let data = '<ion-grid>';

    data = data + '<ion-row>';
    data = data + '<ion-col size-xs="12" size-sm="8">';
    data = data + '<ion-grid>';
    data = data + '<ion-row>';
    data = data + '<h1 style="color:black;">¿QUÉ ES ACCESO TOTAL?</h1>';
    data = data + '</ion-row>';
    data = data + '<ion-row>';
    data = data + '<ion-col>';
    data = data + '<p style="color:black;" align="justify">El plan mensual <b>“ACCESO TOTAL”</b> como su nombre lo dice, te permite el acceso sin restricciones a todas las áreas del gimnasio. Aparatos, instructores, clases, regaderas, lockers, sauna, etc. </p>';
    data = data + '<p style="color:black;" align="justify">Puedes hacer uso de todo el equipo e instalaciones el día y a la hora que tú quieras dentro de nuestro horario de operación.</p>';
    data = data + '<p style="color:black;" align="justify">También, si tienes hijos incluye el servicio de guardería sin ningún costo adicional.</p>';
    data = data + '</ion-col>';
    data = data + '</ion-row>';
    data = data + '<ion-row>';
    data = data + '<h1 style="color: black;">¿QUÉ INCLUYE?</h1>';
    data = data + '</ion-row>';
    data = data + '<ion-row>';
    data = data + '<ion-col>';
    data = data + '<ion-item lines="none">';
    data = data + '<ion-icon slot="start" name="checkmark-circle-outline"></ion-icon>';
    data = data + '<ion-label>APARATOS</ion-label>';
    data = data + '</ion-item>';
    data = data + '</ion-col>';
    data = data + '<ion-col>';
    data = data + '<ion-item lines="none">';
    data = data + '<ion-icon slot="start" name="checkmark-circle-outline"></ion-icon>';
    data = data + '<ion-label>INSTRUCTOR</ion-label>';
    data = data + '</ion-item>';
    data = data + '</ion-col>';
    data = data + '<ion-col>';
    data = data + '<ion-item lines="none">';
    data = data + '<ion-icon slot="start" name="checkmark-circle-outline"></ion-icon>';
    data = data + '<ion-label>REGADERAS</ion-label>';
    data = data + '</ion-item>';
    data = data + '</ion-col>';
    data = data + '<ion-col>';
    data = data + '<ion-item lines="none">';
    data = data + '<ion-icon slot="start" name="checkmark-circle-outline"></ion-icon>';
    data = data + '<ion-label>SAUNA</ion-label>';
    data = data + '</ion-item>';
    data = data + '</ion-col>';
    data = data + '<ion-col>';
    data = data + '<ion-item lines="none">';
    data = data + '<ion-icon slot="start" name="checkmark-circle-outline"></ion-icon>';
    data = data + '<ion-label>LOCKERS</ion-label>';
    data = data + '</ion-item>';
    data = data + '</ion-col>';
    data = data + '<ion-col>';
    data = data + '<ion-item lines="none">';
    data = data + '<ion-icon slot="start" name="checkmark-circle-outline"></ion-icon>';
    data = data + '<ion-label>GUARDERÍA</ion-label>';
    data = data + '</ion-item>';
    data = data + '</ion-col>';
    data = data + '</ion-row>';
    data = data + '</ion-grid>';
    data = data + '</ion-col>';
    data = data + '<ion-col>';
    data = data + '<img src="../../assets/images/img-accesototal.png"/>';
    data = data + '</ion-col>';
    data = data + '</ion-row>';
    data = data + '</ion-grid>';

    return data;
  }

  getSalesReport(initDate: any, endDate: any) {

    const data = [
      {
        saleDate: new Date(2020, 11, 1),
        fullName: 'Mila Kunis',
        gender: 'Femenino',
        birthDate: new Date(1983, 8, 1),
        phone: '664 689-8989',
        membershipType: 'Familiar',
        memberId: 12345,
        vigency: new Date(2020, 12, 1)
      },
      {
        saleDate: new Date(2020, 10, 2),
        fullName: 'Jennifer Lawrence',
        gender: 'Femenino',
        birthDate: new Date(1990, 8, 15),
        phone: '664 689-2323',
        membershipType: 'Lady Fitness',
        memberId: 67890,
        vigency: new Date(2020, 12, 20)
      }

    ];

    return data;
  }

}
