import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  onConfigurationOptionSelected = new BehaviorSubject<string>('block');

  onUpdateData = new BehaviorSubject<string>('');

constructor() { }

  setConfigurationSelection(value: string) {
    this.onConfigurationOptionSelected.next(value);
  }

  setOnUpdateData(value: string) {
    this.onUpdateData.next(value);
  }

}
