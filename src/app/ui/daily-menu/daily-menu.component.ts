import { MenuService } from './../../services/menu.service';
import { Component, OnInit } from '@angular/core';
import { MenuItem } from './../../models/menu-item';
import { ConnectionService, ConnectionStatus } from '../../services/connection.service';

@Component({
  selector: 'app-daily-menu',
  templateUrl: './daily-menu.component.html',
  styleUrls: ['./daily-menu.component.css']
})
export class DailyMenuComponent implements OnInit {

  items: MenuItem[];

  constructor(
    private menu: MenuService,
    private connection: ConnectionService
  ) { }

  ngOnInit() {
    this.connection.status$.subscribe(status => {
      console.log(status);
      if (status == ConnectionStatus.connected) {
        this.menu.todayItems().subscribe( (items: MenuItem[]) => {
          this.items = items;
          console.log(items);
        });
      }
    }, error => {
      console.log('errors')
    })
  }

  imageURL(namespace: string) {
    return this.menu.imageURL(namespace);
  }


}
