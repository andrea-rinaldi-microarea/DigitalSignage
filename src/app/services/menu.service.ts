import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TodayItems } from '../models/today-items';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  constructor(private _http: HttpClient) { }

  public todayItems(): Observable<TodayItems> {
    var today = new Date().toISOString();
    let params = new HttpParams().set("dae", today);
    var items$ = new Observable<TodayItems>( observer => {
      this._http.get('/api/menu/todayItems', {params: params}).subscribe( response => {
        var items = response as TodayItems;
        observer.next(items);
        observer.complete();
      });
    });
    return items$;
  }
}
