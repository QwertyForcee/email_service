import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowQuoteTasksComponent } from './show-quote-tasks.component';

describe('ShowQuoteTasksComponent', () => {
  let component: ShowQuoteTasksComponent;
  let fixture: ComponentFixture<ShowQuoteTasksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowQuoteTasksComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowQuoteTasksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
