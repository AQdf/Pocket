import { TestBed } from '@angular/core/testing';

import { EffectiveDateService } from '../effective-date.service';

describe('EffectiveDateService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EffectiveDateService = TestBed.get(EffectiveDateService);
    expect(service).toBeTruthy();
  });
});
