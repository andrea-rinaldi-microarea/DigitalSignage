import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MenuItem } from '../models/menu-item';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  constructor(private _http: HttpClient) { }

  public todayItems(): Observable<MenuItem[]> {
    var today = new Date();
    today.setDate(today.getDate() + 1);
    let params = new HttpParams().set("date", today.toISOString());
    var items$ = new Observable<MenuItem[]>( observer => {
      this._http.get('/api/menu/todayItems', {params: params}).subscribe( response => {
        var items = response as MenuItem[];
        observer.next(items);
        observer.complete();
      });
    });
    return items$;
  }

  public imageURL(namespace: string) {
    //@@ TODO invoke back-end service
    return "/assets/images/" + namespace;
  }
}
