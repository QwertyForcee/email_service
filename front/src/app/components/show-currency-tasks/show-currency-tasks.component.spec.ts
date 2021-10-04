import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowCurrencyTasksComponent } from './show-currency-tasks.component';

describe('ShowCurrencyTasksComponent', () => {
  let component: ShowCurrencyTasksComponent;
  let fixture: ComponentFixture<ShowCurrencyTasksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowCurrencyTasksComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowCurrencyTasksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
