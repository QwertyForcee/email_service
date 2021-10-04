import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrencyTaskFormComponent } from './currency-task-form.component';

describe('CurrencyTaskFormComponent', () => {
  let component: CurrencyTaskFormComponent;
  let fixture: ComponentFixture<CurrencyTaskFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CurrencyTaskFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrencyTaskFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
