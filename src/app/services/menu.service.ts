import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MenuItem } from '../models/menu-item';
import { WeeklyMenu } from '../models/weekly-menu';

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

  public weekMenu(): Observable<WeeklyMenu> {
    var today = new Date();
    let params = new HttpParams().set("date", today.toISOString());
    var menu$ = new Observable<WeeklyMenu>( observer => {
      this._http.get('/api/menu/weekMenu', {params: params}).subscribe( response => {
        var menu = response as WeeklyMenu;
        observer.next(menu);
        observer.complete();
      });
    });
    return menu$;
  }

  public imageURL(namespace: string): Observable<string> {
    console.log("imageURL " + namespace);
    let params = new HttpParams().set("nmspace", namespace);
    let url$ = new Observable<string> (observer => {
      this._http.get('/api/menu/imageURL', {params: params}).subscribe( response => {
        observer.next(response["url"]);
        observer.complete();
      });
    });
    return url$;
  }
}
