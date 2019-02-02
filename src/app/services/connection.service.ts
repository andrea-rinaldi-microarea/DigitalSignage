import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';

export enum ConnectionStatus { notConnected, connecting, connected, connectionError }

@Injectable({
  providedIn: 'root'
})
export class ConnectionService {

  private _errorMessage : string;

  constructor(private _http: Http) { }

  public connect(): Observable<ConnectionStatus> {
    var connect$ = new Observable<ConnectionStatus>( (observer) => {
      observer.next(ConnectionStatus.connecting);
      this._http.post('/api/loginManager/login', null).subscribe(result => {
        var ok: boolean = result.json() as boolean;
        observer.next(ok ? ConnectionStatus.connected : ConnectionStatus.connectionError);
        this._errorMessage = "Login failed";
        observer.complete();
      }, (error) => {
        observer.next(ConnectionStatus.connectionError);
        this._errorMessage = error.statusText + ' - ' + error._body;
        observer.error(this._errorMessage);
      });
    });
    return connect$;
  }
}
