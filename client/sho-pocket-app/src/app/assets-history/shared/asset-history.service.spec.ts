import { TestBed, inject } from '@angular/core/testing';

import { AssetHistoryService } from './asset-history.service';

describe('AssetHistoryService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AssetHistoryService]
    });
  });

  it('should be created', inject([AssetHistoryService], (service: AssetHistoryService) => {
    expect(service).toBeTruthy();
  }));
});
