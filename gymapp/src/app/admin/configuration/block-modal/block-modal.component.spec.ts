import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { BlockModalComponent } from './block-modal.component';

describe('BlockModalComponent', () => {
  let component: BlockModalComponent;
  let fixture: ComponentFixture<BlockModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BlockModalComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(BlockModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
