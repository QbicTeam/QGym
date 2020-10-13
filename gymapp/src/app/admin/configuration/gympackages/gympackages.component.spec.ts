import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { GympackagesComponent } from './gympackages.component';

describe('GympackagesComponent', () => {
  let component: GympackagesComponent;
  let fixture: ComponentFixture<GympackagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GympackagesComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(GympackagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
