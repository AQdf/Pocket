import { TestBed, inject } from '@angular/core/testing';

import { BalancesTotalService } from './balances-total.service';

describe('BalancesTotalService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BalancesTotalService]
    });
  });

  it('should be created', inject([BalancesTotalService], (service: BalancesTotalService) => {
    expect(service).toBeTruthy();
  }));
});
