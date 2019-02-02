import { ConnectionStatus } from './../../services/connection.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-connection-status',
  templateUrl: './connection-status.component.html',
  styleUrls: ['./connection-status.component.css']
})
export class ConnectionStatusComponent implements OnInit {
  connectionStatus = ConnectionStatus;

  @Input() status: ConnectionStatus;
  
  constructor() { }

  ngOnInit() {
    console.log(this.status);
  }

  

}
