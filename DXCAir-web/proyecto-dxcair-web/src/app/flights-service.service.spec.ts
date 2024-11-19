import { TestBed } from '@angular/core/testing';

import { FlightsServiceService } from './flights-service.service';

describe('FlightsServiceService', () => {
  let service: FlightsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FlightsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
