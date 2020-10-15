import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { CovidalertComponent } from './covidalert.component';

describe('CovidalertComponent', () => {
  let component: CovidalertComponent;
  let fixture: ComponentFixture<CovidalertComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CovidalertComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(CovidalertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
