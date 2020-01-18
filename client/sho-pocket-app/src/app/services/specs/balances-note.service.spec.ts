import { TestBed, inject } from '@angular/core/testing';

import { BalancesNoteService } from '../balances-note.service';

describe('BalancesNoteService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BalancesNoteService]
    });
  });

  it('should be created', inject([BalancesNoteService], (service: BalancesNoteService) => {
    expect(service).toBeTruthy();
  }));
});
