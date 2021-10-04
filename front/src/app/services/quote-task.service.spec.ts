import { TestBed } from '@angular/core/testing';

import { QuoteTaskService } from './quote-task.service';

describe('QuoteTaskService', () => {
  let service: QuoteTaskService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(QuoteTaskService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
