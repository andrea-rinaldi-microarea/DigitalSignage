import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

export enum ConnectionStatus { notConnected, connecting, connected, connectionError }

@Injectable({
  providedIn: 'root'
})
export class ConnectionService {

  public status$: Observable<ConnectionStatus>;
  private _status$: Subject<ConnectionStatus>;

  private _errorMessage : string;

  constructor(private _http: HttpClient) { 
    this._status$ = new Subject<ConnectionStatus>();
    this.status$ = this._status$.asObservable();
  }

  public connect(): Observable<ConnectionStatus> {
    var connect$ = new Observable<ConnectionStatus>( (observer) => {
      observer.next(ConnectionStatus.connecting);
      this._status$.next(ConnectionStatus.connecting);
      this._http.post('/api/loginManager/login', null).subscribe(result => {
        var ok: boolean = result as boolean;
        observer.next(ok ? ConnectionStatus.connected : ConnectionStatus.connectionError);
        this._status$.next(ok ? ConnectionStatus.connected : ConnectionStatus.connectionError);
        this._errorMessage = "Login failed";
        observer.complete();
        this._status$.complete();
      }, (error) => {
        this._status$.next(ConnectionStatus.connectionError);
        observer.next(ConnectionStatus.connectionError);
        this._errorMessage = error.statusText + ' - ' + error.error;
        observer.error(this._errorMessage);
        this._status$.error(this._errorMessage);
      });
    });
    return connect$;
  }

  public disconnect() {
    this._http.post('/api/loginManager/logout', null);
  }
}
