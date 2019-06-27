import { HttpHeaders, HttpParams } from '@angular/common/http';

export abstract class BaseService {

  constructor() { }

  getDefaultOptions() {
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    let params = new HttpParams();
    let options = { headers: headers, params: params };

    return options;
  }
}
