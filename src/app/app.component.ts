import { ConnectionService, ConnectionStatus } from './services/connection.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  
  connectionStatus: ConnectionStatus = ConnectionStatus.notConnected;

  constructor(private connection: ConnectionService) { }

  ngOnInit() {
    this.connection.connect().subscribe( status => {
      this.connectionStatus = status;
    });
  }

}
