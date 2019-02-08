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
    if (this.connection.status == ConnectionStatus.connected) {
      this.downloadMenus();
    } else {
      this.connection.status$.subscribe(status => {
        console.log(status);
        if (status == ConnectionStatus.connected) {
          this.downloadMenus();
        }
      })
    }
  }

  private downloadMenus() {
    this.menu.todayItems().subscribe( (items: MenuItem[]) => {
      this.items = items;
      this.items.forEach(item => {
        this.menu.imageURL(item.picture).subscribe( url => {
          item["imageURL"] = url;
        });
      })
      console.log(items);
    });
  }

}
