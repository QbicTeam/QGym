import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  onConfigurationOptionSelected = new BehaviorSubject<string>('block');

constructor() { }

  setConfigurationSelection(value: string) {
    this.onConfigurationOptionSelected.next(value);
  }

}
