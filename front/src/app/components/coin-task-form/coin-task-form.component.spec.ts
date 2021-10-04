import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoinTaskFormComponent } from './coin-task-form.component';

describe('CoinTaskFormComponent', () => {
  let component: CoinTaskFormComponent;
  let fixture: ComponentFixture<CoinTaskFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoinTaskFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoinTaskFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
