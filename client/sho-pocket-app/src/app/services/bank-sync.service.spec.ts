import { TestBed, inject } from '@angular/core/testing';

import { BankSyncService } from './bank-sync.service';

describe('BankSyncService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BankSyncService]
    });
  });

  it('should be created', inject([BankSyncService], (service: BankSyncService) => {
    expect(service).toBeTruthy();
  }));
});
