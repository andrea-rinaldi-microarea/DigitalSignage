import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  constructor(private _http: Http) { }

  public load(): Observable<any> {
    return this._http.get('/api/menu/headers');
  }
}
