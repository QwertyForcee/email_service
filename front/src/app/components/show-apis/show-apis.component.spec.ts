import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowApisComponent } from './show-apis.component';

describe('ShowApisComponent', () => {
  let component: ShowApisComponent;
  let fixture: ComponentFixture<ShowApisComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowApisComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowApisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
