import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowCoinTasksComponent } from './show-coin-tasks.component';

describe('ShowCoinTasksComponent', () => {
  let component: ShowCoinTasksComponent;
  let fixture: ComponentFixture<ShowCoinTasksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowCoinTasksComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowCoinTasksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
