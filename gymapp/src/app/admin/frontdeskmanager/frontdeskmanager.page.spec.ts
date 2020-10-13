import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { FrontdeskmanagerPage } from './frontdeskmanager.page';

describe('FrontdeskmanagerPage', () => {
  let component: FrontdeskmanagerPage;
  let fixture: ComponentFixture<FrontdeskmanagerPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FrontdeskmanagerPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(FrontdeskmanagerPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
