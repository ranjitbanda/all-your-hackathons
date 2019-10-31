import { TestBed } from '@angular/core/testing';

import { HexService } from './hex.service';

describe('HexService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HexService = TestBed.get(HexService);
    expect(service).toBeTruthy();
  });
});
