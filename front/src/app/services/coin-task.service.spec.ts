import { TestBed } from '@angular/core/testing';

import { CoinTaskService } from './coin-task.service';

describe('CoinTaskService', () => {
  let service: CoinTaskService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CoinTaskService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
