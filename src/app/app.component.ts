import { ConnectionService, ConnectionStatus } from './services/connection.service';
import { Component, OnInit, HostListener, OnDestroy } from '@angular/core';
import { MenuItem } from './models/menu-item';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  
  connectionStatus: ConnectionStatus = ConnectionStatus.notConnected;
  errorMessage: string = null;

  constructor(private connection: ConnectionService) { }

  ngOnInit() {
    this.connection.connect().subscribe( status => {
      this.connectionStatus = status;
    }, error => {
      console.log(error);
      this.errorMessage = "Connection failed: " + error;
    });
  }

  @HostListener('window:beforeunload', ['$event'])
    unloadNotification($event: any) {
        this.connection.disconnect();
    }

    ngOnDestroy() {
      this.connection.disconnect();
    }
}
