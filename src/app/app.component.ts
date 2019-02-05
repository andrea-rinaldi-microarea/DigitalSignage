import { MenuService } from './services/menu.service';
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
  items: MenuItem[];

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
    this.menu.todayItems().subscribe( (items: MenuItem[]) => {
      this.items = items;
      console.log(items);
    })
  }

  imageURL(namespace: string) {
    return this.menu.imageURL(namespace);
  }

  @HostListener('window:beforeunload', ['$event'])
    unloadNotification($event: any) {
        this.connection.disconnect();
    }

    ngOnDestroy() {
      this.connection.disconnect();
    }
}
