import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment'
import { BaseService } from './base.service';
import { BalanceNote } from '../models/balance-note.model';

const balancesNotesApiUrl = environment.baseApiUrl + 'balances/notes/';

@Injectable({
  providedIn: 'root'
})
export class BalancesNoteService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  getNote(effectiveDate: string) {
    return this.http.get(balancesNotesApiUrl + effectiveDate, this.getDefaultOptions());
  }

  patchNote(note: BalanceNote) {
    var body = JSON.stringify(note);
    return this.http.patch(balancesNotesApiUrl + note.effectiveDate, body, this.getDefaultOptions());
  }
}
