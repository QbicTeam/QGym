import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { InitsessionPage } from './initsession.page';

describe('InitsessionPage', () => {
  let component: InitsessionPage;
  let fixture: ComponentFixture<InitsessionPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InitsessionPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(InitsessionPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
