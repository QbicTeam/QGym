import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { BlockusersComponent } from './blockusers.component';

describe('BlockusersComponent', () => {
  let component: BlockusersComponent;
  let fixture: ComponentFixture<BlockusersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BlockusersComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(BlockusersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
