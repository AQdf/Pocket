import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';

import { environment } from '../../environments/environment';
import { BaseService } from './base.service';

const effectiveDateApiUrl = environment.baseApiUrl + 'balances/effective-dates';

@Injectable({
  providedIn: 'root'
})
export class EffectiveDateService extends BaseService {

    private selectedEffectiveDateSource = new Subject<string>();
    private notifyReloadSource = new Subject();

    effectiveDateSelected$ = this.selectedEffectiveDateSource.asObservable();
    effectiveDatesReload$ = this.notifyReloadSource.asObservable();

  constructor(private http: HttpClient) {
    super();
  }

  notifyEffectiveDateSelected(selectedEffectiveDate: string) {
    this.selectedEffectiveDateSource.next(selectedEffectiveDate);
  }

  notifyReload() {
    this.notifyReloadSource.next();
  }

  getEffectiveDates() {
    return this.http.get(effectiveDateApiUrl, this.getDefaultOptions());
  }
}
