import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { FrontdeskPage } from './frontdesk.page';

describe('FrontdeskPage', () => {
  let component: FrontdeskPage;
  let fixture: ComponentFixture<FrontdeskPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FrontdeskPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(FrontdeskPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
