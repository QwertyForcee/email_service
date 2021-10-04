import { TestBed } from '@angular/core/testing';

import { CurrencyTaskService } from './currency-task.service';

describe('CurrencyTaskService', () => {
  let service: CurrencyTaskService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CurrencyTaskService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
