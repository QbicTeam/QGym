import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { MembersactivationPage } from './membersactivation.page';

describe('MembersactivationPage', () => {
  let component: MembersactivationPage;
  let fixture: ComponentFixture<MembersactivationPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MembersactivationPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(MembersactivationPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
