import { MenuService } from './services/menu.service';
import { ConnectionService, ConnectionStatus } from './services/connection.service';
import { Component, OnInit } from '@angular/core';
import { TodayItems } from './models/today-items';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  
  connectionStatus: ConnectionStatus = ConnectionStatus.notConnected;
  errorMessage: string = null;

  constructor(
    private connection: ConnectionService,
    private menu: MenuService
  ) { }

  ngOnInit() {
    this.connection.connect().subscribe( status => {
      this.connectionStatus = status;
    }, error => {
      console.log(error);
      this.errorMessage = "Connection failed: " + error;
    });

  }

  onTodayItemsClicked() {
    this.menu.todayItems().subscribe( (items: TodayItems) => {
      console.log(items);
    })
  }
}
