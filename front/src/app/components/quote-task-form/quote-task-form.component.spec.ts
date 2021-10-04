import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuoteTaskFormComponent } from './quote-task-form.component';

describe('QuoteTaskFormComponent', () => {
  let component: QuoteTaskFormComponent;
  let fixture: ComponentFixture<QuoteTaskFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuoteTaskFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteTaskFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
