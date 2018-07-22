import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
 
import {Summary} from'./summary.model'

@Injectable()
export class SummaryService {
  selectedPeriodSummary: Summary;
  periodSummaryList: Summary[];
  constructor(private http: Http) {}

  postPeriodSummary(emp : Summary){
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({'Content-Type':'application/json'});
    var requestOptions = new RequestOptions({method : RequestMethod.Post,headers : headerOptions});
    return this.http.post('http://localhost:58192/api/Summary',body,requestOptions).pipe(
      map(x => x.json())
    );
  }
 
  putPeriodSummary(id, emp) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });
    return this.http.put('http://localhost:58192/api/Summary/' + id,
      body,
      requestOptions).pipe(
        map(res => res.json())
      );
  }
 
  getPeriodSummaryList(){
    this.http.get('http://localhost:58192/api/Summary').pipe(
      map((data : Response) =>{
        return data.json() as Summary[];
      })
    ).subscribe(x => {
      this.periodSummaryList = x;
    });
  }
 
  deletePeriodSummary(id: number) {
    return this.http.delete('http://localhost:58192/api/Summary/' + id).pipe(map(res => res.json()));
  }
}
